namespace Application.Interfaces;
public interface IActivationTokenService
{
    string GenerateActivationToken(Guid userId, string email);
}
