using Domain.Security.Entities;

namespace Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByValueAsync(string value, CancellationToken cancellationToken = default);
    Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default);
    Task DeleteAsync(RefreshToken token, CancellationToken cancellationToken = default);
}
