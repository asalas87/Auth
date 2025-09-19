using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Notifications.Worker.Infrastructure.Messaging;

public class RabbitMqConsumer
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqConsumer(string hostName, string queueName, Func<string, Task> onMessage)
    {
        var factory = new ConnectionFactory() { HostName = hostName };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            await onMessage(message);

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(queue: queueName,
                             autoAck: false,
                             consumer: consumer);
    }
}
