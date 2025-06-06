namespace Application.Security.Common.Responses;

public class LoginResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }

    public LoginResponse(Guid id, string name, string email, string token, string refreshToken)
    {
        Id = id;
        Name = name;
        Email = email;
        Token = token;
        RefreshToken = refreshToken;
    }
}
