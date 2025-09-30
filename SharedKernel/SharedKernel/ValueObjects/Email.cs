using System.Text.RegularExpressions;

namespace SharedKernel.ValueObjects;

public sealed class Email
{
    private static readonly Regex _regex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty.", nameof(value));

        if (!_regex.IsMatch(value))
            throw new ArgumentException("Invalid email format.", nameof(value));

        return new Email(value.Trim().ToLowerInvariant());
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj)
    {
        if (obj is not Email other) return false;
        return Value == other.Value;
    }

    public override int GetHashCode() => Value.GetHashCode();
}
