using System.Net;
using System.Net.Http.Json;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace Infrastructure.Services;

public class ObservabilityErrorReporter : IErrorReporter
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ObservabilityErrorReporter> _logger;

    public ObservabilityErrorReporter(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ObservabilityErrorReporter> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task ReportAsync(Exception exception, HttpContext context)
    {
        try
        {
            var apiKey = _configuration["Observability:ApiKey"];
            var apiId = _configuration["Observability:ApiId"];
            var baseUrl = _configuration["Observability:BaseUrl"];
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiId) || string.IsNullOrEmpty(baseUrl))
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    _logger.LogError("🔴 Observability configuration missing in Development! Check appsettings.");
                }
                else
                {
                    _logger.LogWarning("Observability configuration missing. Skipping error report.");
                }
                return;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl.TrimEnd('/')}/api/v1/{apiId}/incidents");
            request.Headers.Add("X-API-Key", apiKey);

            var payload = new
            {
                message = exception.Message,
                stackTrace = exception.StackTrace,
                level = "ERROR",
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "unknown",
                context = new
                {
                    method = context.Request.Method,
                    path = context.Request.Path.ToString(),
                    traceId = context.TraceIdentifier,
                    user = context.User?.FindFirst("sub")?.Value ?? "Anonymous"
                }
            };

            request.Content = JsonContent.Create(payload);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();

                // Si es 404, es porque el endpoint no está implementado (temporal)
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogDebug("Observability incidents endpoint not implemented yet (404).");
                    return;
                }

                _logger.LogWarning("Failed to report error to observability. Status: {Status}, Body: {Body}",
                    response.StatusCode, body);
            }
        }
        catch (Exception ex)
        {
            // No queremos que un fallo en el envío rompa la aplicación principal
            _logger.LogError(ex, "Error sending incident to observability platform");
        }
    }
}
