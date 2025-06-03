using Domain.Security.Entities;

namespace Application.Security.Services;

public interface IRefreshTokenService
{
    Task<RefreshToken> GenerateAsync(string token, DateTime expiresOn, Guid userId);
    Task<bool> ValidateAsync(string token);
    Task RevokeAsync(string token);
    Task<RefreshToken?> GetByValueAsync(string value, CancellationToken cancellationToken = default);
}
