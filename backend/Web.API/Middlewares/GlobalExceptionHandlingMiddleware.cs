using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;
    private readonly IErrorReporter _errorReporter;

    public GlobalExceptionHandlingMiddleware(
        ILogger<GlobalExceptionHandlingMiddleware> logger,
        IHostEnvironment environment,
        IErrorReporter errorReporter)
    {
        _logger = logger;
        _environment = environment;
        _errorReporter = errorReporter;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception. Method: {Method} Path: {Path} TraceId: {TraceId} User: {User}",
                context.Request.Method,
                context.Request.Path,
                context.TraceIdentifier,
                context.User.Identity?.Name ?? "Anonymous");

            _ = _errorReporter.ReportAsync(ex, context);

            if (context.Response.HasStarted)
                throw;

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";

            ProblemDetails problem = new()
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://httpstatuses.com/500",
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred."
            };

            if (_environment.IsDevelopment())
            {
                problem.Extensions["exception"] = ex.Message;
                problem.Extensions["stackTrace"] = ex.StackTrace;
            }

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
