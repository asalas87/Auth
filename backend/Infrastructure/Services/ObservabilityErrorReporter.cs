using Application.Interfaces;
using Infrastructure.Services.Observability;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class ObservabilityErrorReporter : IErrorReporter
{
    private readonly IObservabilityClient _client;
    private readonly ILogger<ObservabilityErrorReporter> _logger;

    public ObservabilityErrorReporter(
        IObservabilityClient client,
        ILogger<ObservabilityErrorReporter> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task ReportAsync(Exception exception, HttpContext context)
    {
        var payload = new IncidentPayload
        {
            Message = exception.Message,
            Level = "ERROR",
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "unknown",
            StackTrace = exception.StackTrace,
            ExceptionType = exception.GetType().Name,
            Source = "GlobalExceptionHandler",
            Context = new
            {
                method = context.Request.Method,
                path = context.Request.Path.ToString(),
                traceId = context.TraceIdentifier,
                user = context.User?.FindFirst("sub")?.Value ?? "Anonymous"
            }
        };

        await _client.SendIncidentAsync(payload, context.RequestAborted);
    }

    public async Task ReportSecurityEventAsync(
        string eventType,
        string message,
        string level,
        object context,
        CancellationToken cancellationToken = default)
    {
        var payload = new IncidentPayload
        {
            Message = message,
            Level = level,
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "unknown",
            ExceptionType = eventType,
            Source = "SecurityGuardFilter",
            Context = context
        };

        await _client.SendIncidentAsync(payload, cancellationToken);
    }
}
