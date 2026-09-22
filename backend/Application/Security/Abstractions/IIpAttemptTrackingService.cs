using Application.Security.Abstractions;

namespace Application.Security.Abstractions;

public interface IIpAttemptTrackingService
{
    Task<FailureTrackingEntry> GetOrCreateEntryAsync(
        string ip,
        string actionName,
        CancellationToken cancellationToken = default);

    Task<FailureTrackingEntry> RegisterFailureAsync(
        string ip,
        string actionName,
        CancellationToken cancellationToken = default);

    Task ResetAsync(
        string ip,
        string actionName,
        CancellationToken cancellationToken = default);

    Task<bool> IsBlockedAsync(
        string ip,
        string actionName,
        CancellationToken cancellationToken = default);
}
