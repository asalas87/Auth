using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Notifications.Worker.Domain.Events;
using RabbitMQ.Client;
using Xunit;

namespace Notifications.Worker.IntegrationTests.RabbitMq
{
    public class DocumentUploadedConsumerTests
    {
        [Fact(Skip = "Requires RabbitMQ running on localhost")]
        public async Task Should_Publish_And_Consume_DocumentUploadedEvent()
        {
            // Arrange
            var factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var exchange = "documents.test";
            var queue = "documents.test.queue";
            var routingKey = "document.uploaded";

            channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: false);
            channel.QueueDeclare(queue, durable: false, exclusive: false, autoDelete: true);
            channel.QueueBind(queue, exchange, routingKey);

            var testEvent = new DocumentUploadedEvent
            {
                DocumentId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Email = "destinatario@ejemplo.com",
                UploadedAt = DateTime.UtcNow
            };

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(testEvent));

            // Act → publicar
            channel.BasicPublish(exchange, routingKey, null, body);

            // Assert → consumir
            var result = channel.BasicGet(queue, autoAck: true);
            Assert.NotNull(result);

            var message = Encoding.UTF8.GetString(result.Body.ToArray());
            var deserialized = JsonSerializer.Deserialize<DocumentUploadedEvent>(message);

            Assert.Equal(testEvent.DocumentId, deserialized?.DocumentId);
        }
    }
}
