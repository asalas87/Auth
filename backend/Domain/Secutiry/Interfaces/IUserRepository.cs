using Domain.Security.Entities;
using Domain.ValueObjects;

namespace Domain.Secutiry.Interfaces;
public interface IUserRepository
{
    Task<List<User>> GetAll();
    Task<User?> GetByIdAsync(UserId id);
    Task<User?> GetByNameAsync(string idName);
    Task<User?> GetByEmailAsync(Email email);
    Task<bool> ExistsAsync(UserId id);
    Task AddAsync(User user);
    void Update(User user);
    void Delete(User user);
    Task<(List<User> Users, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? filter);
}
