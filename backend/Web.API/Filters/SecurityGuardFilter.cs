using Application.Interfaces;
using Application.Security.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Web.API.Extensions;
using Web.API.Security;

namespace Web.API.Filters;

public class SecurityGuardFilter(
    IIpAttemptTrackingService tracking,
    ITurnstileValidator turnstile,
    IErrorReporter errorReporter,
    ILogger<SecurityGuardFilter> logger,
    ProtectedActionResolver resolver) : IAsyncActionFilter
{
    private readonly IIpAttemptTrackingService _tracking = tracking;
    private readonly ITurnstileValidator _turnstile = turnstile;
    private readonly IErrorReporter _errorReporter = errorReporter;
    private readonly ILogger<SecurityGuardFilter> _logger = logger;
    private readonly ProtectedActionResolver _resolver = resolver;

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var actionName = _resolver.Resolve(context);
        var options = _resolver.GetOptions(actionName);

        if (!_resolver.IsProtected(actionName) || options == null)
        {
            await next();
            return;
        }

        var ip = ClientIpResolver.Resolve(context.HttpContext);

        var entry = await _tracking.GetOrCreateEntryAsync(
            ip,
            actionName,
            context.HttpContext.RequestAborted);

        if (!CheckBlocked(context, ip, actionName, entry))
            return;

        if (options.Mode == "RateLimit")
        {
            var executed = await HandleRateLimitAsync(context, next, ip, actionName, entry, options);
            return;
        }

        if (!await CheckCaptchaAsync(context, ip, actionName, entry, options))
        {
            return;
        }

        var executedContext = await next();

        await HandleFailureTrackingResultAsync(executedContext, ip, actionName, options);
    }

    private async Task<ActionExecutedContext> HandleRateLimitAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next,
        string ip,
        string actionName,
        FailureTrackingEntry entry,
        SecurityActionOptions options)
    {
        lock (entry)
        {
            var now = DateTime.UtcNow;

            if (entry.LastAttempt.HasValue &&
                now - entry.LastAttempt.Value > TimeSpan.FromMinutes(options.TimeWindowMinutes))
            {
                entry.Attempts = 0;
                entry.BlockedUntil = null;
            }

            entry.Attempts++;
            entry.LastAttempt = now;

            if (entry.Attempts > options.MaxAttempts)
            {
                entry.BlockedUntil = now.AddMinutes(options.BlockDurationMinutes);
            }
        }

        if (entry.BlockedUntil.HasValue && entry.BlockedUntil.Value > DateTime.UtcNow)
        {
            _logger.LogWarning("Blocked IP {Ip} attempted to send user report", ip);
            var retryAfterSeconds = Math.Max(
                1,
                (int)(entry.BlockedUntil.Value - DateTime.UtcNow).TotalSeconds);

            context.HttpContext.Response.Headers.RetryAfter = retryAfterSeconds.ToString();
            context.Result = ProblemResult(
                ApiProblemDetailsFactory.TooManyRequests(entry.BlockedUntil.Value),
                StatusCodes.Status429TooManyRequests);
            return null!;
        }

        return await next();
    }

    private async Task HandleFailureTrackingResultAsync(
        ActionExecutedContext executedContext,
        string ip,
        string actionName,
        SecurityActionOptions options)
    {
        var statusCode = executedContext.Result switch
        {
            StatusCodeResult result => result.StatusCode,
            ObjectResult result => result.StatusCode,
            _ => null
        };

        if (statusCode is null)
            return;

        if (statusCode is >= 200 and < 300)
        {
            if (options.ResetOnSuccess)
            {
                await _tracking.ResetAsync(
                    ip,
                    actionName,
                    executedContext.HttpContext.RequestAborted);
            }
            return;
        }

        if (statusCode != StatusCodes.Status401Unauthorized)
            return;

        await _tracking.RegisterFailureAsync(
            ip,
            actionName,
            executedContext.HttpContext.RequestAborted);
    }

    private bool CheckBlocked(
        ActionExecutingContext context,
        string ip,
        string actionName,
        FailureTrackingEntry entry)
    {
        if (!entry.BlockedUntil.HasValue ||
            entry.BlockedUntil.Value <= DateTime.UtcNow)
        {
            return true;
        }

        _logger.LogWarning(
            "Blocked IP {Ip} attempted to access {Action}",
            ip,
            actionName);

        _ = _errorReporter.ReportSecurityEventAsync(
            eventType: "SecurityEvent.IpBlocked",
            message: $"IP {ip} blocked on '{actionName}' after {entry.Attempts} failed attempts.",
            level: "WARN",
            context: new
            {
                eventType = "IP_BLOCKED",
                ip,
                action = actionName,
                attempts = entry.Attempts,
                blockedUntil = entry.BlockedUntil.Value.ToString("o"),
                traceId = context.HttpContext.TraceIdentifier
            },
            cancellationToken: context.HttpContext.RequestAborted);

        var retryAfterSeconds = Math.Max(
            1,
            (int)(entry.BlockedUntil.Value - DateTime.UtcNow).TotalSeconds);

        context.HttpContext.Response.Headers.RetryAfter =
            retryAfterSeconds.ToString();

        context.Result = ProblemResult(
            ApiProblemDetailsFactory.TooManyRequests(
                entry.BlockedUntil.Value),
            StatusCodes.Status429TooManyRequests);

        return false;
    }

    private async Task<bool> CheckCaptchaAsync(
        ActionExecutingContext context,
        string ip,
        string actionName,
        FailureTrackingEntry entry,
        SecurityActionOptions options)
    {
        var captchaRequired =
            CaptchaRequiredPolicy.IsRequired(entry, options);

        var captchaToken = ExtractCaptchaToken(context);

        if (captchaRequired &&
            string.IsNullOrWhiteSpace(captchaToken))
        {
            context.Result = ProblemResult(
                ApiProblemDetailsFactory.CaptchaRequired(),
                StatusCodes.Status428PreconditionRequired);

            return false;
        }

        if (string.IsNullOrWhiteSpace(captchaToken))
            return true;

        try
        {
            var isValid = await _turnstile.ValidateTokenAsync(
                captchaToken,
                context.HttpContext.RequestAborted);

            if (!isValid)
            {
                _logger.LogWarning(
                    "Invalid Turnstile token from IP {Ip} for action {Action}",
                    ip,
                    actionName);

                context.Result = ProblemResult(
                    ApiProblemDetailsFactory.InvalidCaptcha(),
                    StatusCodes.Status400BadRequest);

                return false;
            }
        }
        catch (TurnstileUnavailableException)
        {
            _logger.LogWarning(
                "Turnstile service unavailable for IP {Ip} action {Action}",
                ip,
                actionName);

            context.Result = ProblemResult(
                ApiProblemDetailsFactory.ServiceUnavailable(),
                StatusCodes.Status503ServiceUnavailable);

            return false;
        }

        return true;
    }

    private static ObjectResult ProblemResult(
        ProblemDetails problem,
        int statusCode)
    {
        return new ObjectResult(problem)
        {
            StatusCode = statusCode
        };
    }

    private static string? ExtractCaptchaToken(
        ActionExecutingContext context)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
                continue;

            var property = argument
                .GetType()
                .GetProperty("CaptchaToken");

            if (property is not null)
                return property.GetValue(argument) as string;
        }

        return null;
    }
}