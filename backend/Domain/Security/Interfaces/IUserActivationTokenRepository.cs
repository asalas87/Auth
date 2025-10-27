using Domain.Security.Entities;

namespace Domain.Security.Interfaces;
public interface IUserActivationTokenRepository
{
    Task AddAsync(UserActivationToken token);
    void Update(UserActivationToken token);
    Task<UserActivationToken?> GetByTokenAsync(string token);
}
