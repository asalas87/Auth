using Application.Security.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Web.API.Extensions;
using Web.API.Security;

namespace Web.API.Filters;

public class SecurityGuardFilter(
    IIpAttemptTrackingService tracking,
    ITurnstileValidator turnstile,
    CaptchaRequiredPolicy captchaPolicy,
    ILogger<SecurityGuardFilter> logger) : IAsyncActionFilter
{
    private readonly IIpAttemptTrackingService _tracking = tracking;
    private readonly ITurnstileValidator _turnstile = turnstile;
    private readonly CaptchaRequiredPolicy _captchaPolicy = captchaPolicy;
    private readonly ILogger<SecurityGuardFilter> _logger = logger;

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var actionName = ProtectedActionResolver.Resolve(context);

        if (!ProtectedActionResolver.IsProtected(actionName))
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

        if (!await CheckCaptchaAsync(
                context,
                ip,
                actionName,
                entry))
        {
            return;
        }

        var executedContext = await next();

        await UpdateTrackingAsync(
            executedContext,
            ip,
            actionName);
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
        FailureTrackingEntry entry)
    {
        var captchaRequired =
            _captchaPolicy.IsRequired(actionName, entry);

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

    private async Task UpdateTrackingAsync(
        ActionExecutedContext executedContext,
        string ip,
        string actionName)
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
            await _tracking.ResetAsync(
                ip,
                actionName,
                executedContext.HttpContext.RequestAborted);

            return;
        }

        if (statusCode != StatusCodes.Status401Unauthorized)
            return;

        await _tracking.RegisterFailureAsync(
            ip,
            actionName,
            executedContext.HttpContext.RequestAborted);
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
