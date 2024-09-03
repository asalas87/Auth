using Application.Interfaces;
namespace Infrastructure.Services;
public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    public bool VerifyPassword(string password, string userPassword) => BCrypt.Net.BCrypt.Verify(password, userPassword);
}
