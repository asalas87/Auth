using Application.Security.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Security;

public class IpAttemptTrackingService(
    IMemoryCache cache,
    IConfiguration configuration) : IIpAttemptTrackingService
{
    private readonly IMemoryCache _cache = cache;
    private readonly IConfiguration _configuration = configuration;

    private int MaxAttempts => _configuration.GetValue<int?>("Security:RateLimiting:MaxAttempts") ?? 5;
    private int TimeWindowMinutes => _configuration.GetValue<int?>("Security:RateLimiting:TimeWindowMinutes") ?? 15;
    private int BlockDurationMinutes => _configuration.GetValue<int?>("Security:RateLimiting:BlockDurationMinutes") ?? 60;

    private string CacheKey(string ip, string actionName) => $"throttle_{ip}_{actionName}";

    public Task<FailureTrackingEntry> GetOrCreateEntryAsync(string ip, string actionName, CancellationToken cancellationToken = default)
    {
        var key = CacheKey(ip, actionName);
        if (!_cache.TryGetValue(key, out FailureTrackingEntry? entry))
        {
            entry = new FailureTrackingEntry();
            var options = new MemoryCacheEntryOptions();
            options.SetSlidingExpiration(TimeSpan.FromMinutes(TimeWindowMinutes));
            _cache.Set(key, entry, options);
        }

        return Task.FromResult(entry!);
    }

    public async Task IncrementFailuresAsync(string ip, string actionName, CancellationToken cancellationToken = default)
    {
        var entry = await GetOrCreateEntryAsync(ip, actionName, cancellationToken);

        if (entry.BlockedUntil.HasValue && entry.BlockedUntil.Value > DateTime.UtcNow)
        {
            return;
        }

        if (entry.LastAttempt.HasValue && DateTime.UtcNow - entry.LastAttempt.Value > TimeSpan.FromMinutes(TimeWindowMinutes))
        {
            entry.Attempts = 0;
            entry.BlockedUntil = null;
        }

        entry.Attempts++;
        entry.LastAttempt = DateTime.UtcNow;

        if (entry.Attempts >= MaxAttempts)
        {
            entry.BlockedUntil = DateTime.UtcNow.AddMinutes(BlockDurationMinutes);
        }
    }

    public Task ResetAsync(string ip, string actionName, CancellationToken cancellationToken = default)
    {
        var key = CacheKey(ip, actionName);
        _cache.Remove(key);
        return Task.CompletedTask;
    }

    public async Task<bool> IsBlockedAsync(string ip, string actionName, CancellationToken cancellationToken = default)
    {
        var entry = await GetOrCreateEntryAsync(ip, actionName, cancellationToken);

        if (entry.BlockedUntil.HasValue && entry.BlockedUntil.Value > DateTime.UtcNow)
        {
            return true;
        }

        if (entry.LastAttempt.HasValue && DateTime.UtcNow - entry.LastAttempt.Value > TimeSpan.FromMinutes(TimeWindowMinutes))
        {
            entry.Attempts = 0;
            entry.BlockedUntil = null;
        }

        return false;
    }
}
