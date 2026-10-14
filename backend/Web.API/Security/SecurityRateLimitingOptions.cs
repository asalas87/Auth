namespace Web.API.Security;

public class SecurityRateLimitingOptions
{
    public Dictionary<string, SecurityActionOptions> Actions { get; set; } = new();
}