using Domain.Security.Entities;

namespace Domain.Security.Interfaces;
public interface IUserActivationTokenRepository
{
    Task AddAsync(UserActivationToken token, CancellationToken cancellationToken);
    void Update(UserActivationToken token, CancellationToken cancellationToken);
    Task<UserActivationToken?> GetByTokenAsync(string token, CancellationToken cancellationToken);
}
