using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.ResetPassword;

public record RequestPasswordResetCommand(string Email) : IRequest<ErrorOr<bool>>;
