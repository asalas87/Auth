using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Observability;

public class ObservabilityClient : IObservabilityClient
{
    private readonly HttpClient _httpClient;
    private readonly ObservabilityOptions _options;
    private readonly ILogger<ObservabilityClient> _logger;

    public ObservabilityClient(
        HttpClient httpClient,
        IOptions<ObservabilityOptions> options,
        ILogger<ObservabilityClient> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendIncidentAsync(IncidentPayload payload, CancellationToken ct = default)
    {
        if (!_options.IsConfigured)
        {
            _logger.LogWarning("Observability configuration missing. Skipping incident.");
            return;
        }

        try
        {
            var url = $"{_options.BaseUrl.TrimEnd('/')}/api/v1/{_options.ApiId}/incidents";

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("X-API-Key", _options.ApiKey);

            // StringContent envía Content-Length (no Transfer-Encoding: chunked),
            // evitando que ModSecurity bloquee con la regla 920181.
            var json = JsonSerializer.Serialize(payload);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request, ct);
            if (response.IsSuccessStatusCode) return;

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogDebug("Observability incidents endpoint not implemented (404).");
                return;
            }

            var body = await response.Content.ReadAsStringAsync(ct);
            _logger.LogWarning(
                "Failed to report incident. Status: {Status}, Body: {Body}",
                response.StatusCode,
                body);
        }
        catch (Exception ex)
        {
            // Fire-and-forget: nunca debe romper la app principal.
            _logger.LogError(ex, "Error sending incident to Observability");
        }
    }
}
