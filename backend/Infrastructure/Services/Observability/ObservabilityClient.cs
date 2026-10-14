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

    public Task SendIncidentAsync(IncidentPayload payload, CancellationToken ct = default)
    => PostAsync("/api/v1/{apiId}/incidents", payload, "incident", ct);

    public Task SendUserReportAsync(UserReportPayload payload, CancellationToken ct = default)
    => PostAsync("/api/v1/ingest/user-reports", payload, "user report", ct);

    private async Task PostAsync(
    string pathTemplate,
    object payload,
    string contextLabel,
    CancellationToken ct)
    {
        if (!_options.IsConfigured)
        {
            _logger.LogWarning("Observability configuration missing. Skipping {Context}.", contextLabel);
            return;
        }

        try
        {
            var path = pathTemplate.Replace("{apiId}", _options.ApiId);
            var url = $"{_options.BaseUrl.TrimEnd('/')}{path}";

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("X-API-Key", _options.ApiKey);

            var json = JsonSerializer.Serialize(payload);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request, ct);
            if (response.IsSuccessStatusCode) return;

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogDebug("Observability {Context} endpoint not implemented (404).", contextLabel);
                return;
            }

            var body = await response.Content.ReadAsStringAsync(ct);
            _logger.LogWarning(
            "Failed to report {Context}. Status: {Status}, Body: {Body}",
            contextLabel,
            response.StatusCode,
            body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending {Context} to Observability", contextLabel);
        }
    }
}
