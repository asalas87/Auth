using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Notifications.Worker.Domain.Events;
using Notifications.Worker.Domain.Enums;
using Notifications.Worker.Infrastructure.Persistence;
using Notifications.Worker.Application.Handlers;
using Notifications.Worker.Application.Services;

public class SqlNotificationWorker : BackgroundService
{
    private readonly ILogger<SqlNotificationWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly DocumentUploadedHandler _uploadedHandler;
    private readonly DocumentExpiringHandler _expiringHandler;

    public SqlNotificationWorker(
        ILogger<SqlNotificationWorker> logger,
        IServiceScopeFactory scopeFactory,
        NotificationService service)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _uploadedHandler = new DocumentUploadedHandler(service);
        _expiringHandler = new DocumentExpiringHandler(service);
    }   

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<NotificationsDbContext>();

                var pending = await db.Notifications
                    .Where(n => n.Status == NotificationStatus.Pending)
                    .OrderBy(n => n.CreatedAt)
                    .Take(10) // batch pequeño
                    .ToListAsync(stoppingToken);

                foreach (var notif in pending)
                {
                    try
                    {
                        notif.MarkProcessing();

                        switch (notif.Type)
                        {
                            case NotificationType.DocumentUploaded:
                                await _uploadedHandler.HandleAsync(new DocumentUploadedEvent
                                {
                                    DocumentId = notif.RelatedDocumentId ?? Guid.Empty,
                                    UserId = Guid.Empty, // opcional, según tu modelo
                                    Email = notif.RecipientEmail,
                                    UploadedAt = notif.CreatedAt
                                });
                                notif.MarkSent();
                                break;

                            case NotificationType.DocumentExpiring:
                                await _expiringHandler.HandleAsync(new DocumentExpiringEvent
                                {
                                    DocumentId = notif.RelatedDocumentId ?? Guid.Empty,
                                    UserId = Guid.Empty,
                                    Email = notif.RecipientEmail,
                                    ExpirationDate = notif.ExpirationDate // o usa otra columna si tenés fecha real
                                });
                                notif.MarkSent();
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error procesando notificación {Id}", notif.Id);
                        notif.MarkFailed();
                    }
                }

                await db.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el loop principal de SqlNotificationWorker");
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
