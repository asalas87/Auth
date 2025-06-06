using Application.Security.Services;
using Domain.Security.Entities;
using Application.Interfaces;
using Domain.Primitives;

namespace Infrastructure.Security;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RefreshToken> GenerateAsync(string token, DateTime expiresOn, Guid userId)
    {
        var refreshToken = new RefreshToken(token, expiresOn, new UserId(userId));

        await _refreshTokenRepository.AddAsync(refreshToken);
        await _unitOfWork.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<bool> ValidateAsync(string token)
    {
        var refreshToken = await _refreshTokenRepository
            .GetByValueAsync(token);

        return refreshToken != null && !refreshToken.IsExpired && !refreshToken.IsRevoked;
    }

    public async Task RevokeAsync(string token)
    {
        var refreshToken = await _refreshTokenRepository.GetByValueAsync(token);

        if (refreshToken is not null)
        {
            refreshToken.Revoke();
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<RefreshToken?> GetByValueAsync(string value, CancellationToken cancellationToken = default)
    {
        return await _refreshTokenRepository.GetByValueAsync(value);
    }
}
