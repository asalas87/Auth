using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.ResetPassword;

public record ResetPasswordCommand(
    string Email,
    string Password,
    string ConfirmPassword,
    string Token
) : IRequest<ErrorOr<bool>>;
