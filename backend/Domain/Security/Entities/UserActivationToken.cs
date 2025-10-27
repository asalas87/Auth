using Domain.Primitives;

namespace Domain.Security.Entities;

public class UserActivationToken : AggergateRoot<Guid>
{
    public UserId UserId { get; private set; } = null!;
    public string Token { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }
    public bool Used { get; private set; }

    private UserActivationToken() { }

    public static UserActivationToken Create(UserId userId, TimeSpan validityPeriod)
    {
        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("+", "").Replace("/", "").Replace("=", "");
        return new UserActivationToken
        {
            UserId = userId,
            Token = token,
            ExpiresAt = DateTime.UtcNow.Add(validityPeriod),
            Used = false
        };
    }

    public bool IsValid() => !Used && DateTime.UtcNow < ExpiresAt;

    public void MarkAsUsed() => Used = true;
}
