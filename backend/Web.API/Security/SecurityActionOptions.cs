namespace Web.API.Security;

public class SecurityActionOptions
{
    public string Mode { get; set; } = "FailureTracking";
    public int MaxAttempts { get; set; } = 6;
    public int TimeWindowMinutes { get; set; } = 15;
    public int BlockDurationMinutes { get; set; } = 15;
    public int CaptchaRequiredAttempts { get; set; } = 3;
    public bool CaptchaAlwaysRequired { get; set; } = false;
    public bool ResetOnSuccess { get; set; } = true;
}