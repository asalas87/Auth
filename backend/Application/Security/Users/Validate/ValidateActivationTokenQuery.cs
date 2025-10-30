using ErrorOr;
using MediatR;

namespace Application.Security.Users.Validate;

public record ValidateActivationTokenQuery(string Token) : IRequest<ErrorOr<ValidateActivationTokenResult>>;

public record ValidateActivationTokenResult(Guid UserId, string Email);
