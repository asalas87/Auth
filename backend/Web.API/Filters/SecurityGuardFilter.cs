using Application.Security.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Web.API.Filters;

public class SecurityGuardFilter(
    IIpAttemptTrackingService tracking,
    ITurnstileValidator turnstile,
    IConfiguration configuration,
    ILogger<SecurityGuardFilter> logger) : IAsyncActionFilter
{
    private readonly IIpAttemptTrackingService _tracking = tracking;
    private readonly ITurnstileValidator _turnstile = turnstile;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<SecurityGuardFilter> _logger = logger;

    private int MaxAttempts => _configuration.GetValue<int?>("Security:RateLimiting:MaxAttempts") ?? 5;
    private int CaptchaRequiredAttempts => _configuration.GetValue<int?>("Security:RateLimiting:CaptchaRequiredAttempts") ?? 3;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var ip = GetClientIp(context.HttpContext);
        var actionName = GetActionName(context);

        if (!IsProtectedAction(actionName))
        {
            await next();
            return;
        }

        var entry = await _tracking.GetOrCreateEntryAsync(ip, actionName, context.HttpContext.RequestAborted);

        if (entry.BlockedUntil.HasValue && entry.BlockedUntil.Value > DateTime.UtcNow)
        {
            _logger.LogWarning("Blocked IP {Ip} attempted to access {Action}", ip, actionName);
            context.Result = new StatusCodeResult(StatusCodes.Status429TooManyRequests);
            return;
        }

        bool captchaRequired = IsCaptchaRequired(actionName, entry);
        string? captchaToken = ExtractCaptchaToken(context);

        if (captchaRequired && string.IsNullOrWhiteSpace(captchaToken))
        {
            context.Result = new JsonResult(new { requiresCaptcha = true }) { StatusCode = StatusCodes.Status428PreconditionRequired };
            return;
        }

        if (!string.IsNullOrWhiteSpace(captchaToken))
        {
            try
            {
                bool isValid = await _turnstile.ValidateTokenAsync(captchaToken, context.HttpContext.RequestAborted);
                if (!isValid)
                {
                    _logger.LogWarning("Invalid Turnstile token from IP {Ip} for action {Action}", ip, actionName);
                    context.Result = new BadRequestObjectResult(new { errors = new[] { "Captcha inválido." } });
                    return;
                }
            }
            catch (TurnstileUnavailableException)
            {
                _logger.LogWarning("Turnstile service unavailable for IP {Ip} action {Action}", ip, actionName);
                context.Result = new StatusCodeResult(StatusCodes.Status503ServiceUnavailable);
                return;
            }
        }

        var executedContext = await next();

        if (executedContext.Result is StatusCodeResult statusCodeResult && statusCodeResult.StatusCode is int statusCode)
        {
            if (statusCode >= 200 && statusCode < 300)
            {
                await _tracking.ResetAsync(ip, actionName, context.HttpContext.RequestAborted);
            }
            else if (statusCode >= 400)
            {
                await _tracking.IncrementFailuresAsync(ip, actionName, context.HttpContext.RequestAborted);
            }
        }
        else if (executedContext.Result is ObjectResult objectResult && objectResult.StatusCode is int objectStatusCode)
        {
            if (objectStatusCode >= 200 && objectStatusCode < 300)
            {
                await _tracking.ResetAsync(ip, actionName, context.HttpContext.RequestAborted);
            }
            else if (objectStatusCode >= 400)
            {
                await _tracking.IncrementFailuresAsync(ip, actionName, context.HttpContext.RequestAborted);
            }
        }
    }

    private static string GetClientIp(HttpContext httpContext)
    {
        if (httpContext.Request.Headers.TryGetValue("CF-Connecting-IP", out var cfIp))
        {
            return cfIp.ToString();
        }

        return httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }

    private static string GetActionName(ActionExecutingContext context)
    {
        var action = context.ActionDescriptor.RouteValues["action"];
        return action?.ToLowerInvariant() switch
        {
            "register" => "register",
            "login" => "login",
            "forgotpassword" => "forgot-password",
            "resetpassword" => "reset-password",
            _ => action?.ToLowerInvariant() ?? "unknown"
        };
    }

    private static bool IsProtectedAction(string actionName)
    {
        return actionName is "register" or "login" or "forgot-password" or "reset-password";
    }

    private static bool IsCaptchaRequired(string actionName, FailureTrackingEntry entry)
    {
        if (actionName is "forgot-password" or "reset-password" or "register")
            return true;

        if (actionName == "login" && entry.Attempts >= 3)
            return true;

        return false;
    }

    private static string? ExtractCaptchaToken(ActionExecutingContext context)
    {
        foreach (var arg in context.ActionArguments.Values)
        {
            if (arg is null) continue;

            var prop = arg.GetType().GetProperty("CaptchaToken");
            if (prop != null)
            {
                var value = prop.GetValue(arg) as string;
                return value;
            }
        }

        return null;
    }
}
