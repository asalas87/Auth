using Microsoft.AspNetCore.Mvc;

namespace Web.API.Middlewares;

public class GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger, IHostEnvironment environment) : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger = logger;
    private readonly IHostEnvironment _environment = environment;

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
                """
                Unhandled exception.
                Method: {Method}
                Path: {Path}
                TraceId: {TraceId}
                User: {User}
                """,
                context.Request.Method,
                context.Request.Path,
                context.TraceIdentifier,
                context.User.Identity?.Name ?? "Anonymous");

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
