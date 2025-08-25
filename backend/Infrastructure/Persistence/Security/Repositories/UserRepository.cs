using Domain.Security.Entities;
using Domain.Secutiry.Interfaces;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Security.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddAsync(User user) => await _context.Users.AddAsync(user);
    public void Delete(User user) => _context.Users.Remove(user);
    public void Update(User user) => _context.Users.Update(user);
    public async Task<bool> ExistsAsync(UserId id) => await _context.Users.AnyAsync(user => user.Id == id);
    public async Task<User?> GetByIdAsync(UserId id) => await _context.Users.Include(u => u.Company).Include(u => u.Role).SingleOrDefaultAsync(c => c.Id == id);
    public async Task<List<User>> GetAll() => await _context.Users.ToListAsync();

    public async Task<User?> GetByNameAsync(string idName) => await _context.Users.SingleOrDefaultAsync(c => c.Name == idName);
    public async Task<User?> GetByEmailAsync(Email email) => await _context.Users.Include(d => d.RefreshTokens).Include(u => u.Company).Include(u => u.Role).SingleOrDefaultAsync(c => c.Email == email);
    public async Task<(List<User> Users, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? filter)
    {
        var query = _context.Users.Include(d => d.RefreshTokens).Include(u => u.Role).Include(u => u.Company).AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(u => u.Name.Contains(filter) || u.Email.Value.Contains(filter));
        }

        var totalCount = await query.CountAsync();

        var users = await query
            .OrderBy(u => u.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (users, totalCount);
    }
}
