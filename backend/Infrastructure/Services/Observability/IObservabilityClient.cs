namespace Infrastructure.Services.Observability;

public interface IObservabilityClient
{
    Task SendIncidentAsync(IncidentPayload payload, CancellationToken ct = default);
}
