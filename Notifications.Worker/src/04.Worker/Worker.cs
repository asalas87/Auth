using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notifications.Worker.Domain.Events;
using Notifications.Worker.Application.Handlers;
using Notifications.Worker.Application.Services;
using Notifications.Worker.Infrastructure.Email;
using Notifications.Worker.Infrastructure.Messaging;
using System.Text.Json;

namespace Notifications.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly DocumentUploadedHandler _uploadedHandler;
    private readonly DocumentExpiringHandler _expiringHandler;

    public Worker(ILogger<Worker> logger, NotificationService service)
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
