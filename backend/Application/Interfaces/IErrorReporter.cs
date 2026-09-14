using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IErrorReporter
{
    Task ReportAsync(Exception exception, HttpContext context);
}
