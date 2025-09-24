using Notifications.Worker.Domain.Events;
using Notifications.Worker.Application.Handlers;
using Notifications.Worker.Application.Services;
using Notifications.Worker.Infrastructure.Messaging;
using System.Text.Json;

namespace Notifications.Worker;

public class RabbitNotificationWorker : BackgroundService
{
    private readonly ILogger<RabbitNotificationWorker> _logger;
    private readonly DocumentUploadedHandler _uploadedHandler;
    private readonly DocumentExpiringHandler _expiringHandler;

    public RabbitNotificationWorker(ILogger<RabbitNotificationWorker> logger, NotificationService service)
    {
        _logger = logger;
        _uploadedHandler = new DocumentUploadedHandler(service);
        _expiringHandler = new DocumentExpiringHandler(service);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        new RabbitMqConsumer("localhost", "document.uploaded", async msg =>
        {
            var evt = JsonSerializer.Deserialize<DocumentUploadedEvent>(msg);
            if (evt != null) await _uploadedHandler.HandleAsync(evt);
        });

        new RabbitMqConsumer("localhost", "document.expiring", async msg =>
        {
            var evt = JsonSerializer.Deserialize<DocumentExpiringEvent>(msg);
            if (evt != null) await _expiringHandler.HandleAsync(evt);
        });

        return Task.CompletedTask;
    }
}
