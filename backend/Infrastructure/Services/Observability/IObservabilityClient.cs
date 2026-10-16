namespace Infrastructure.Services.Observability;

public interface IObservabilityClient
{
    Task SendIncidentAsync(IncidentPayload payload, CancellationToken ct = default);
    Task SendUserReportAsync(UserReportPayload payload, CancellationToken ct = default);
}
