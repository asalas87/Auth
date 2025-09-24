using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Worker.Domain.Interfaces;
using Notifications.Worker.Infrastructure.Email;
using Notifications.Worker.Infrastructure.Persistence.Repositories;

namespace Notifications.Worker.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        return services;
    }
}
