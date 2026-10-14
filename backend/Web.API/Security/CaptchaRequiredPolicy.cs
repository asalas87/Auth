using Application.Security.Abstractions;

namespace Web.API.Security;

public static class CaptchaRequiredPolicy
{
    public static bool IsRequired(FailureTrackingEntry entry, SecurityActionOptions options)
    {
        if (options.CaptchaAlwaysRequired) return true;
        return entry.Attempts >= options.CaptchaRequiredAttempts;
    }
}
