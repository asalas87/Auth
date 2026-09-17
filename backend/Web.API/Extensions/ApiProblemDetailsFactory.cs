using Microsoft.AspNetCore.Mvc;

namespace Web.API.Extensions;

public static class ApiProblemDetailsFactory
{
    public static ProblemDetails CaptchaRequired(string detail = "Se requiere verificación CAPTCHA.")
    {
        return Create(
            StatusCodes.Status428PreconditionRequired,
            "Captcha Required",
            detail,
            "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            new Dictionary<string, object> { ["requiresCaptcha"] = true });
    }

    public static ProblemDetails TooManyRequests(DateTime blockedUntil)
    {
        return Create(
            StatusCodes.Status429TooManyRequests,
            "Too Many Requests",
            $"La IP está bloqueada temporalmente hasta las {blockedUntil:HH:mm} UTC.",
            "https://tools.ietf.org/html/rfc9110#section-15.5.6",
            new Dictionary<string, object>
            {
                ["blockedUntil"] = blockedUntil.ToString("o") // ISO 8601 con zona horaria
            });
    }

    public static ProblemDetails InvalidCaptcha(string detail = "El token de CAPTCHA no es válido o ha expirado.")
    {
        return Create(
            StatusCodes.Status400BadRequest,
            "Invalid Captcha",
            detail);
    }

    public static ProblemDetails ServiceUnavailable(string detail = "El servicio de verificación no está disponible. Inténtalo más tarde.")
    {
        return Create(
            StatusCodes.Status503ServiceUnavailable,
            "Service Unavailable",
            detail);
    }

    private static ProblemDetails Create(
        int statusCode,
        string title,
        string detail,
        string? type = null,
        IDictionary<string, object>? extensions = null)
    {
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Type = type ?? $"https://httpstatuses.com/{statusCode}"
        };

        if (extensions != null)
        {
            foreach (var (key, value) in extensions)
            {
                problem.Extensions[key] = value;
            }
        }

        return problem;
    }
}
