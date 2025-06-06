using Domain.Security.Entities;

namespace Application.Security.Services;

public interface IAuthenticationService
{
    Task<string> GenerateRefreshTokenAsync(Guid userId);
    string GenerateAccessToken(User user);
}