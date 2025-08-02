using Domain.Security.Entities;

namespace Domain.Security.Interfaces;
public interface IRoleRepository
{
    Task<List<Role>> GetAll();
    Task<Role?> GetByIdAsync(int id);
}

