using Domain.Partners.Entities;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Security.Entities;

public sealed class User : AggergateRoot<UserId>
{
    public User(string name, string password, Email email, Role role, bool active)
    {
        Name = name;
        Password = password;
        Active = active;
        Email = email;
        Role = role;
    }
    public User(string name, string password, Email email, Role role, Company company, bool active)
    {
        Name = name;
        Password = password;
        Active = active;
        Email = email;
        Role = role;
        Company = company;
    }
    public User()
    {
    }
    public string Name { get; private set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Email Email { get; private set; } = default!;
    public Role Role { get; private set; } = null!;
    public bool Active { get; set; }
    public Company? Company { get; private set; } = null!;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
    public void Update(string name, Email email, Role role, Company company)
    {
        Name = name;
        Email = email;
        Role = role;
        Company = company;
    }
}
