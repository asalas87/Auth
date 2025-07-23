using Domain.Security.Entities;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.Delete
{
    public sealed record DeleteUserCommand(UserId UserId) : IRequest<ErrorOr<Unit>>;
}
