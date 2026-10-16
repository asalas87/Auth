using Domain.Security.Entities;
using Domain.Security.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Security.Repositories;
public class UserActivationTokenRepository(ApplicationDbContext context) : IUserActivationTokenRepository
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    public async Task AddAsync(UserActivationToken token, CancellationToken cancellationToken) => await _context.UserActivationTokens.AddAsync(token, cancellationToken);

    public async Task<UserActivationToken?> GetByTokenAsync(string token, CancellationToken cancellationToken) => await _context.UserActivationTokens
        .FirstOrDefaultAsync(t => t.Token == token, cancellationToken);

    public void Update(UserActivationToken token, CancellationToken cancellationToken) => _context.UserActivationTokens.Update(token);
    public async Task InvalidateOtherTokensAsync(UserId userId, string currentToken, CancellationToken cancellationToken)
    {
        var tokens = await _context.UserActivationTokens
            .Where(t => t.UserId == userId && t.Token != currentToken && !t.Used && t.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        foreach (var token in tokens)
        {
            token.MarkAsUsed();
        }
    }
}
