using Application.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Middlewares;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = httpContext.TraceIdentifier;
        var method = httpContext.Request.Method;
        var path = httpContext.Request.Path;

        logger.LogError(
            exception,
            "Unhandled exception. Method: {Method} Path: {Path} TraceId: {TraceId}",
            method,
            path,
            traceId);

        var errorReporter = httpContext.RequestServices
            .GetRequiredService<IErrorReporter>();

        await errorReporter.ReportAsync(exception, httpContext);

        var problemDetails = BuildProblemDetails(exception, method, path, traceId);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }

    private ProblemDetails BuildProblemDetails(
        Exception exception,
        string method,
        string path,
        string traceId)
    {
        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Detail = environment.IsDevelopment()
                ? exception.Message
                : "An unexpected error occurred.",
            Instance = $"{method} {path}",
            Type = "https://httpstatuses.com/500"
        };

        problem.Extensions["traceId"] = traceId;

        if (environment.IsDevelopment())
        {
            problem.Extensions["exceptionType"] = exception.GetType().Name;
            problem.Extensions["stackTrace"] = exception.StackTrace;
        }

        return problem;
    }
}
