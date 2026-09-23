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
}
