using Domain.Security.Entities;
using Domain.Security.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Security.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<Role>> GetAll() => await _context.Roles.ToListAsync();

    public async Task<Role?> GetByIdAsync(int id) => await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
}
