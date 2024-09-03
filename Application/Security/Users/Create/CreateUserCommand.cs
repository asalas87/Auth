using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.Create
{
    public record CreateUserCommand(
        string Name,
        Email Email,
        string Password,
        string ConfirmPassword) : IRequest<ErrorOr<Guid>>;
}
