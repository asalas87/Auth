using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IErrorReporter
{
    Task ReportAsync(Exception exception, HttpContext context);
    Task ReportSecurityEventAsync(
        string eventType,
        string message,
        string level,
        object context,
        CancellationToken cancellationToken = default);
    Task ReportUserReportAsync(
        string message,
        string? userEmail,
        string? userId,
        string? traceId,
        string? url,
        string? userAgent,
        CancellationToken cancellationToken = default);
}
