namespace Infrastructure.Services.Observability;

public class ObservabilityOptions
{
    public const string SectionName = "Observability";

    public string BaseUrl { get; set; } = string.Empty;
    public string ApiId { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string UserAgent { get; set; } = "Auth-API/1.0";
    public int TimeoutSeconds { get; set; } = 10;

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(BaseUrl) &&
        !string.IsNullOrWhiteSpace(ApiId) &&
        !string.IsNullOrWhiteSpace(ApiKey);
}
