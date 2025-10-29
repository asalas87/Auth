using System.Security.Claims;
using Application.Interfaces;

namespace Infrastructure.Services;
public class ActivationTokenService(IJwtTokenGenerator jwtTokenGenerator) : IActivationTokenService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

    public string GenerateActivationToken(Guid userId, string email)
    {
        var claims = new[]
        {
        new Claim("uid", userId.ToString()),
        new Claim("email", email),
        new Claim("type", "activation")
    };

        return _jwtTokenGenerator.GenerateToken(claims, DateTime.UtcNow.AddHours(24));
    }
}
