using Domain.Secutiry.Entities;
using Domain.ValueObjects;

namespace Domain.Secutiry.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<User?> GetByIdAsync(UserId id);
        Task<User?> GetByNameAsync(string idName);
        Task<User?> GetByEmailAsync(Email email);
        Task<bool> ExistsAsync(UserId id);
        Task AddAsync(User customer);
        void Update(User customer);
        void Delete(User customer);
    }
}
