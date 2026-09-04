using System.Text;
using RabbitMQ.Client;

namespace HRMS.Employee.Infrastructure.Messaging;

public class RabbitMqPublisher
{
    private readonly ConnectionFactory _connectionFactory;

    public RabbitMqPublisher()
    {
        _connectionFactory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
    }

    public async Task PublishAsync(string exchangeName,string routingKey,string message)
    {
        await using var connection = await _connectionFactory.CreateConnectionAsync();

        await using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(exchangeName,ExchangeType.Direct,durable: true);

        var body = Encoding.UTF8.GetBytes(message);

        var properties = new BasicProperties
        {
            Persistent = true,
            ContentType = "application/json"
        };

        await channel.BasicPublishAsync(exchangeName,routingKey,mandatory: false,
                                        basicProperties: properties,body: body);
    }
}