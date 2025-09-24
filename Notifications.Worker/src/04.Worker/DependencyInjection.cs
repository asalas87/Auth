namespace Notifications.Worker.Worker;
public static class DependencyInjection
{
    public static IServiceCollection AddWorker(this IServiceCollection services, IConfiguration config)
    {
        // registrar servicios de aplicación
        return services;
    }
}
