using HRMS.Contracts.Employee;
using HRMS.Payroll.Application.IntegrationEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace HRMS.Payroll.Infrastructure.Messaging;

public sealed class PayrollRabbitMqConsumer : BackgroundService
{
    private readonly ConnectionFactory _connectionFactory;
    private readonly IServiceScopeFactory _scopeFactory;

    public PayrollRabbitMqConsumer(IServiceScopeFactory scopeFactory)
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

        await channel.QueueDeclareAsync("payroll.employee",durable: true,exclusive: false,autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = ea.Body.ToArray();

            var message = Encoding.UTF8.GetString(body);

            var integrationEvent =
                JsonSerializer.Deserialize<EmployeeCreatedIntegrationEvent>(
                    message);

            if (integrationEvent is null)
            {
                throw new InvalidOperationException(
                    "Unable to deserialize EmployeeCreatedIntegrationEvent.");
            }

            using var scope = _scopeFactory.CreateScope();

            var handler = scope.ServiceProvider.GetRequiredService<PayrollEmployeeCreatedHandler>();

            await handler.HandleAsync(integrationEvent);

            await channel.BasicAckAsync(
                ea.DeliveryTag,
                multiple: false);
        };

        await channel.BasicConsumeAsync("payroll.employee",autoAck: false,consumer);

        await Task.Delay(Timeout.Infinite,stoppingToken);
    }
}