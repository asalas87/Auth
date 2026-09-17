using Application.Security.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
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

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var actionName = ProtectedActionResolver.Resolve(context);
        if (!ProtectedActionResolver.IsProtected(actionName))
        {
            await next();
            return;
        }

        var ip = ClientIpResolver.Resolve(context.HttpContext);
        var entry = await _tracking.GetOrCreateEntryAsync(ip, actionName, context.HttpContext.RequestAborted);

        if (!CheckBlocked(context, ip, actionName, entry)) return;
        if (!await CheckCaptchaAsync(context, ip, actionName, entry)) return;

        var executedContext = await next();
        await UpdateTrackingAsync(executedContext, ip, actionName);
    }

    private bool CheckBlocked(ActionExecutingContext context, string ip, string actionName, FailureTrackingEntry entry)
    {
        if (!entry.BlockedUntil.HasValue || entry.BlockedUntil.Value <= DateTime.UtcNow)
            return true;

        _logger.LogWarning("Blocked IP {Ip} attempted to access {Action}", ip, actionName);

        // RFC 9110: Retry-After en segundos restantes
        var retryAfterSeconds = Math.Max(1, (int)(entry.BlockedUntil.Value - DateTime.UtcNow).TotalSeconds);
        context.HttpContext.Response.Headers.RetryAfter = retryAfterSeconds.ToString();

        context.Result = ProblemResult(
            ApiProblemDetailsFactory.TooManyRequests(entry.BlockedUntil.Value),
            StatusCodes.Status429TooManyRequests);
        return false;
    }

    private async Task<bool> CheckCaptchaAsync(ActionExecutingContext context, string ip, string actionName, FailureTrackingEntry entry)
    {
        bool captchaRequired = _captchaPolicy.IsRequired(actionName, entry);
        string? captchaToken = ExtractCaptchaToken(context);

        if (captchaRequired && string.IsNullOrWhiteSpace(captchaToken))
        {
            await _tracking.IncrementFailuresAsync(ip, actionName, context.HttpContext.RequestAborted);

            if (entry.BlockedUntil.HasValue && entry.BlockedUntil.Value > DateTime.UtcNow)
            {
                return CheckBlocked(context, ip, actionName, entry);
            }

            context.Result = ProblemResult(
                ApiProblemDetailsFactory.CaptchaRequired(),
                StatusCodes.Status428PreconditionRequired);
            return false;
        }

        if (string.IsNullOrWhiteSpace(captchaToken))
            return true;

        try
        {
            bool isValid = await _turnstile.ValidateTokenAsync(captchaToken, context.HttpContext.RequestAborted);
            if (!isValid)
            {
                _logger.LogWarning("Invalid Turnstile token from IP {Ip} for action {Action}", ip, actionName);
                context.Result = ProblemResult(
                    ApiProblemDetailsFactory.InvalidCaptcha(),
                    StatusCodes.Status400BadRequest);
                return false;
            }
        }
        catch (TurnstileUnavailableException)
        {
            _logger.LogWarning("Turnstile service unavailable for IP {Ip} action {Action}", ip, actionName);
            context.Result = ProblemResult(
                ApiProblemDetailsFactory.ServiceUnavailable(),
                StatusCodes.Status503ServiceUnavailable);
            return false;
        }

        return true;
    }

    private async Task UpdateTrackingAsync(ActionExecutedContext executedContext, string ip, string actionName)
    {
        var statusCode = executedContext.Result switch
        {
            StatusCodeResult s => s.StatusCode,
            ObjectResult o => o.StatusCode,
            _ => (int?)null
        };

        if (statusCode is null) return;

        if (statusCode is >= 200 and < 300)
            await _tracking.ResetAsync(ip, actionName, executedContext.HttpContext.RequestAborted);
        else if (statusCode >= 400)
            await _tracking.IncrementFailuresAsync(ip, actionName, executedContext.HttpContext.RequestAborted);
    }

    private static ObjectResult ProblemResult(ProblemDetails problem, int statusCode)
        => new(problem) { StatusCode = statusCode };

    private static string? ExtractCaptchaToken(ActionExecutingContext context)
    {
        foreach (var arg in context.ActionArguments.Values)
        {
            if (arg is null) continue;

            var prop = arg.GetType().GetProperty("CaptchaToken");
            if (prop != null)
                return prop.GetValue(arg) as string;
        }

        return null;
    }
}
