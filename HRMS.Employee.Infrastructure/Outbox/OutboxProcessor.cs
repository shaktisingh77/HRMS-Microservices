using System.Text.Json;
using HRMS.Contracts.IntegrationEvents;
using HRMS.Employee.Infrastructure.Messaging;
using HRMS.Employee.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HRMS.Employee.Infrastructure.Outbox;

public sealed class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxProcessor> _logger;

    public OutboxProcessor(IServiceScopeFactory scopeFactory, ILogger<OutboxProcessor> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();
            //var dispatcher = scope.ServiceProvider.GetRequiredService<IIntegrationEventDispatcher>();

            var publisher = scope.ServiceProvider.GetRequiredService<RabbitMqPublisher>();

            var messages = await dbContext.OutboxMessages
                                          .Where(x => x.ProcessedOnUtc == null && x.RetryCount < 3)
                                          .OrderBy(x => x.OccurredOnUtc)
                                          .Take(20)
                                          .ToListAsync(stoppingToken);

            foreach (var message in messages)
            {
                try
                {
                    var eventType = Type.GetType(message.Type);

                    if (eventType is null)
                        throw new InvalidOperationException($"Unable to resolve event type: {message.Type}");

                    var integrationEvent =
                        JsonSerializer.Deserialize(message.Payload,eventType) as IIntegrationEvent;

                    if (integrationEvent is null)
                        throw new InvalidOperationException($"Unable to deserialize event: {message.Id}");

                    await publisher.PublishAsync("hrms.events",eventType.Name,message.Payload);

                    message.MarkAsProcessed();
                }
                catch (Exception ex)
                {
                    message.MarkAsFailed(ex.Message);

                    _logger.LogError(ex,"Failed to process OutboxMessage {MessageId}",message.Id);
                }
            }

            await dbContext.SaveChangesAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(5),stoppingToken);
        }
    }
}