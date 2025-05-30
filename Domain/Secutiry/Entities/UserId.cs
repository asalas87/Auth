namespace Domain.Security.Entities;
public sealed class UserId : IEquatable<UserId>
{
    public Guid Value { get; }

    public UserId(Guid value)
    {
        Value = value;
    }

    public override bool Equals(object? obj) => obj is UserId other && Equals(other);
    public bool Equals(UserId? other) => other != null && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
