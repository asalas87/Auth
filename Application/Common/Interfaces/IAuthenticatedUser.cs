namespace Application.Common.Interfaces;

public interface IAuthenticatedUser
{
    Guid? UserId { get; }
}
