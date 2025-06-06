using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Security.Entities
{
    public sealed class User : AggergateRoot<UserId>
    {
        public User(UserId id, string name, string password, Email email, bool active)
        {
            Id = id;
            Name = name;
            Password = password;
            Active = active;
            Email = email;
        }
        public User()
        {
        }
        public string Name { get; private set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Email Email { get; private set; } = default!;
        public bool Active { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
        public static User UpdateUser(Guid id, string name, string password, Email email, bool active)
        {
            return new User(new UserId(id), name, password, email, active);
        }
    }
}
