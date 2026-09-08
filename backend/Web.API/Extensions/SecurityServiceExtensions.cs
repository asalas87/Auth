using Application.Security.Abstractions;
using Infrastructure.Security;
using Microsoft.Extensions.Caching.Memory;
using Web.API.Filters;

namespace Web.API.Extensions;

public static class SecurityServiceExtensions
{
    public static IServiceCollection AddSecurityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IIpAttemptTrackingService>(sp =>
        {
            var cache = sp.GetRequiredService<IMemoryCache>();
            return new IpAttemptTrackingService(cache, configuration);
        });

        services.AddScoped<ITurnstileValidator, TurnstileValidator>();
        services.AddScoped<SecurityGuardFilter>();

        return services;
    }

    public static IMvcBuilder AddSecurityGuardFilter(this IMvcBuilder builder)
    {
        builder.AddMvcOptions(options =>
        {
            options.Filters.Add<SecurityGuardFilter>();
        });

        return builder;
    }
}
