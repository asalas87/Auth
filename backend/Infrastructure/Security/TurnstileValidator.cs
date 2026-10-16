using Application.Security.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Security;

public class TurnstileValidator(IConfiguration configuration) : ITurnstileValidator
{
    private readonly IConfiguration _configuration = configuration;

    private string SecretKey => _configuration["Security:Turnstile:SecretKey"] 
        ?? throw new InvalidOperationException("Turnstile SecretKey is not configured.");
    private string VerifyUrl => _configuration["Security:Turnstile:VerifyUrl"] 
        ?? "https://challenges.cloudflare.com/turnstile/v0/siteverify";

    public async Task<bool> ValidateTokenAsync(string token, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(token))
            return false;

        try
        {
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, VerifyUrl);
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["secret"] = SecretKey,
                ["response"] = token
            });

            using var response = await client.SendAsync(request, ct);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var json = await response.Content.ReadAsStringAsync(ct);
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;

            if (root.TryGetProperty("success", out var successElement))
            {
                return successElement.GetBoolean();
            }

            return false;
        }
        catch (HttpRequestException ex)
        {
            throw new TurnstileUnavailableException("Turnstile service is unavailable.", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new TurnstileUnavailableException("Turnstile service timed out.", ex);
        }
    }
}
