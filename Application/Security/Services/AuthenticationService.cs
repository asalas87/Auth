using Domain.Primitives;
using Domain.Security.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Security.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _configuration;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationService(
        IConfiguration configuration,
        IRefreshTokenService refreshTokenService,
        IUnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _refreshTokenService = refreshTokenService;
        _unitOfWork = unitOfWork;
    }

    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new("sub", user.Id.Value.ToString()),
            new("name", user.Name!),
            new("email", user.Email.Value)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateRefreshTokenAsync(Guid userId)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var expiresOn = DateTime.UtcNow.AddDays(7);

        await _refreshTokenService.GenerateAsync(token, expiresOn, userId);

        return token;
    }
}
