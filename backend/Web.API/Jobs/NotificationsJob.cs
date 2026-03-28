using Application.Documents.Services;

namespace Web.Api.Jobs;

public class NotificationsJob : BackgroundService
{
    private readonly ILogger<NotificationsJob> _logger;
    private readonly IServiceProvider _serviceProvider;

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

            var delay = GetDelayUntilNextRun();
            _logger.LogInformation("Próxima ejecución en {Delay}", delay);

            await Task.Delay(delay, stoppingToken);
        }

        _logger.LogInformation("NotificationsJob detenido.");
    }

    private TimeSpan GetDelayUntilNextRun()
    {
        var now = DateTime.Now;

        var nextRun = new DateTime(
            now.Year,
            now.Month,
            now.Day,
            9, 0, 0
        );

        if (now >= nextRun)
        {
            nextRun = nextRun.AddDays(1);
        }

        return nextRun - now;
    }
}
