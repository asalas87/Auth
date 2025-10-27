using Domain.Security.Entities;
using Domain.Security.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Security.Repositories;
public class UserActivationTokenRepository(ApplicationDbContext context) : IUserActivationTokenRepository
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    public async Task AddAsync(UserActivationToken token) => await _context.UserActivationTokens.AddAsync(token);

    public async Task<UserActivationToken?> GetByTokenAsync(string token) => await _context.UserActivationTokens
        .FirstOrDefaultAsync(t => t.Token == token);

    public void Update(UserActivationToken token) => _context.UserActivationTokens.Update(token);
}
