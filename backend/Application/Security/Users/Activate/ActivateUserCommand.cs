using ErrorOr;
using MediatR;

namespace Application.Security.Users.Activate;

public record ActivateUserCommand(Guid UserId, string Password, string Token) : IRequest<ErrorOr<Guid>>;
