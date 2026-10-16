using Application.Common.Interfaces;
using Application.Interfaces;
using Infrastructure.Services.Observability;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class ObservabilityErrorReporter : IErrorReporter
{
    private readonly IObservabilityClient _client;
    private readonly ILogger<ObservabilityErrorReporter> _logger;
    private readonly IAuthenticatedUser _authenticatedUser;

    public ObservabilityErrorReporter(
        IObservabilityClient client,
        IAuthenticatedUser authenticatedUser,
        ILogger<ObservabilityErrorReporter> logger)
    {
        _client = client;
        _logger = logger;
        _authenticatedUser = authenticatedUser;
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
                user = _authenticatedUser.UserId?.ToString() ?? "Anonymous"
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

    public async Task ReportUserReportAsync(
        string message,
        string? userEmail,
        string? userId,
        string? traceId,
        string? url,
        string? userAgent,
        CancellationToken cancellationToken = default)
    {
        var payload = new UserReportPayload
        {
            Message = message,
            UserEmail = userEmail,
            UserId = userId,
            TraceId = traceId,
            Url = url,
            UserAgent = userAgent
        };

        await _client.SendUserReportAsync(payload, cancellationToken);
    }
}
