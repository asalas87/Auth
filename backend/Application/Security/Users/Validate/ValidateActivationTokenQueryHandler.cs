using Domain.Security.Entities;
using Domain.Security.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.Validate;

public sealed class ValidateActivationTokenQueryHandler(
    IUserActivationTokenRepository tokenRepository,
    IUserRepository userRepository
) : IRequestHandler<ValidateActivationTokenQuery, ErrorOr<ValidateActivationTokenResult>>
{
    private readonly IUserActivationTokenRepository _tokenRepository = tokenRepository;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<ErrorOr<ValidateActivationTokenResult>> Handle(
        ValidateActivationTokenQuery request,
        CancellationToken cancellationToken)
    {
        var token = await _tokenRepository.GetByTokenAsync(request.Token);

        if (token is null)
            return Error.NotFound("ActivationToken.NotFound", "El token de activación no existe.");

        if (!token.IsValid())
            return Error.Validation("ActivationToken.Invalid", "El token de activación ha expirado o ya fue usado.");

        var user = await _userRepository.GetByIdAsync(token.UserId);

        if (user is null)
            return Error.NotFound("User.NotFound", "No se encontró el usuario asociado al token.");

        return new ValidateActivationTokenResult(user.Id.Value, user.Email.Value);
    }
}
