using System.Text.Json.Serialization;

namespace Infrastructure.Services.Observability;

public record IncidentPayload
{
    [JsonPropertyName("message")]
    public required string Message { get; init; }

    [JsonPropertyName("level")]
    public required string Level { get; init; }

    [JsonPropertyName("environment")]
    public string Environment { get; init; } = "unknown";

    [JsonPropertyName("stackTrace")]
    public string? StackTrace { get; init; }

    [JsonPropertyName("exceptionType")]
    public string? ExceptionType { get; init; }

    [JsonPropertyName("source")]
    public string? Source { get; init; }

    [JsonPropertyName("context")]
    public object? Context { get; init; }
}
