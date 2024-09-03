using Domain.Secutiry.Entities;

public interface IAuthenticationService
{
    string GenerateToken(User user);
}