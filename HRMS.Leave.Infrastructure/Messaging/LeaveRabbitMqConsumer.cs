using System.Text;
using System.Text.Json;
using HRMS.Contracts.Employee;
using HRMS.Leave.Application.IntegrationEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HRMS.Leave.Infrastructure.Messaging;

public sealed class LeaveRabbitMqConsumer : BackgroundService
{
    private readonly ConnectionFactory _connectionFactory;
    private readonly IServiceScopeFactory _scopeFactory;

    public LeaveRabbitMqConsumer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        _connectionFactory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var connection = await _connectionFactory.CreateConnectionAsync();

        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync("leave.employee",durable: true,exclusive: false,autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = ea.Body.ToArray();

            var message = Encoding.UTF8.GetString(body);

            var integrationEvent = JsonSerializer.Deserialize<EmployeeCreatedIntegrationEvent>(message);

            if (integrationEvent is null)
            {
                throw new InvalidOperationException("Unable to deserialize EmployeeCreatedIntegrationEvent.");
            }

            using var scope = _scopeFactory.CreateScope();

            var handler = scope.ServiceProvider.GetRequiredService<LeaveEmployeeCreatedHandler>();

            await handler.HandleAsync(integrationEvent);

            await channel.BasicAckAsync(ea.DeliveryTag,multiple: false);
        };

        await channel.BasicConsumeAsync("leave.employee",autoAck: false,consumer);

        await Task.Delay(Timeout.Infinite,stoppingToken);
    }
}