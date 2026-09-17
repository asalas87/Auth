using Application.Security.Abstractions;

namespace Web.API.Security;

public sealed class CaptchaRequiredPolicy
{
    private static readonly HashSet<string> AlwaysRequired = new(StringComparer.OrdinalIgnoreCase)
    {
        "forgot-password",
        "reset-password",
        "register"
    };

    private readonly int _captchaRequiredAttempts;

    public CaptchaRequiredPolicy(IConfiguration configuration)
    {
        _captchaRequiredAttempts = configuration.GetValue<int?>("Security:RateLimiting:CaptchaRequiredAttempts") ?? 3;
    }

    public bool IsRequired(string actionName, FailureTrackingEntry entry)
    {
        if (AlwaysRequired.Contains(actionName))
            return true;

        if (string.Equals(actionName, "login", StringComparison.OrdinalIgnoreCase))
            return entry.Attempts >= _captchaRequiredAttempts;

        return false;
    }
}
