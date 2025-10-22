using Domain.Primitives;

namespace Domain.Security.Entities;

public class RefreshToken : Entity<Guid>
{
    public string Token { get; private set; } = null!;
    public DateTime ExpiresOn { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? RevokedOn { get; private set; }
    public bool IsRevoked => RevokedOn.HasValue;
    public bool IsExpired => DateTime.UtcNow >= ExpiresOn;

    public UserId UserId { get; set; } = null!;
    public User User { get; set; } = null!;

    protected RefreshToken() { }
    public RefreshToken(string token, DateTime expiresOn, UserId user)
    {
        Id = Guid.NewGuid();
        Token = token;
        ExpiresOn = expiresOn;
        CreatedOn = DateTime.UtcNow;
        UserId = user;
    }

    public void Revoke()
    {
        RevokedOn = DateTime.UtcNow;
    }
}
