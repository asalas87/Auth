namespace Application.Security.Common.Responses;

public class TokenResponse
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
