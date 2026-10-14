using System.Text.Json.Serialization;

namespace Infrastructure.Services.Observability;

public record UserReportPayload
{
    [JsonPropertyName("message")]
    public required string Message { get; init; }

    [JsonPropertyName("userEmail")]
    public string? UserEmail { get; init; }

    [JsonPropertyName("userId")]
    public string? UserId { get; init; }

    [JsonPropertyName("traceId")]
    public string? TraceId { get; init; }

    [JsonPropertyName("url")]
    public string? Url { get; init; }

    [JsonPropertyName("userAgent")]
    public string? UserAgent { get; init; }
}