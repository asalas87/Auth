using Application.Security.Abstractions;
using Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Web.API.Filters;
using Web.API.Security;

namespace Web.API.Extensions;

public static class SecurityServiceExtensions
{
    public static IServiceCollection AddSecurityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SecurityRateLimitingOptions>(
            configuration.GetSection("Security:RateLimiting"));

        services.AddSingleton<IIpAttemptTrackingService>(sp =>
        {
            var cache = sp.GetRequiredService<IMemoryCache>();
            return new IpAttemptTrackingService(cache, configuration);
        });

        services.AddScoped<ITurnstileValidator, TurnstileValidator>();
        services.AddScoped<SecurityGuardFilter>();
        services.AddScoped<ProtectedActionResolver>();
        services.Configure<MvcOptions>(options =>
        {
            options.Filters.Add<SecurityGuardFilter>();
        });

        return services;
    }
}
