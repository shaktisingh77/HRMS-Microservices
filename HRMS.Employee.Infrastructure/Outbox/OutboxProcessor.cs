using System.Text.Json;
using HRMS.Contracts.IntegrationEvents;
using HRMS.Employee.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HRMS.Employee.Infrastructure.Outbox;

public sealed class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public OutboxProcessor(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();

            var dispatcher = scope.ServiceProvider.GetRequiredService<IIntegrationEventDispatcher>();

            var messages = await dbContext.OutboxMessages
                                          .Where(x => x.ProcessedOnUtc == null)
                                          .OrderBy(x => x.OccurredOnUtc)
                                          .Take(20)
                                          .ToListAsync(stoppingToken);

            foreach (var message in messages)
            {
                var eventType = Type.GetType(message.Type);

                if (eventType is null)
                    continue;

                var integrationEvent = JsonSerializer.Deserialize(message.Payload,eventType) as IIntegrationEvent;

                if (integrationEvent is null)
                    continue;

                await dispatcher.DispatchAsync(integrationEvent);

                message.MarkAsProcessed();
            }

            await dbContext.SaveChangesAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(5),stoppingToken);
        }
    }
}