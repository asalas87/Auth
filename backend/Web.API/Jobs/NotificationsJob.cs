using Application.Documents.Services;

namespace Web.Api.Jobs;

public class NotificationsJob : BackgroundService
{
    private readonly ILogger<NotificationsJob> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(5); // cada 5 min

    public NotificationsJob(ILogger<NotificationsJob> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("NotificationsJob iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                var created = await notificationService.CreateExpiringDocumentNotificationsAsync(stoppingToken);
                var sent = await notificationService.SendPendingNotificationsAsync(stoppingToken);

                if (created > 0 || sent > 0)
                {
                    _logger.LogInformation("Notificaciones procesadas. Creadas: {Created}, Enviadas: {Sent}", created, sent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar NotificationsJob.");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("NotificationsJob detenido.");
    }
}
