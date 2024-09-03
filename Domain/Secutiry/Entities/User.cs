using Domain.Primitives;
using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Domain.Secutiry.Entities
{
    public sealed class User : AggergateRoot
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
        public UserId Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Email Email { get; private set; }
        public bool Active { get; set; }
        public static User UpdateUser(Guid id, string name, string password, Email email, bool active)
        {
            return new User(new UserId(id), name, password, email, active);
        }
    }
}
