namespace Application.Security.Abstractions;

public interface ITurnstileValidator
{
    Task<bool> ValidateTokenAsync(string token, CancellationToken ct = default);
}
