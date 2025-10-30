using Application.Interfaces;
using Domain.Primitives;
using Domain.Security.Entities;
using Domain.Security.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.Activate;

public sealed class ActivateUserCommandHandler(
    IUserRepository userRepository,
    IUserActivationTokenRepository tokenRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork
) : IRequestHandler<ActivateUserCommand, ErrorOr<Guid>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserActivationTokenRepository _tokenRepository = tokenRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Guid>> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
    {
        var token = await _tokenRepository.GetByTokenAsync(request.Token, cancellationToken);
        if (token is null)
            return Error.NotFound("ActivationToken.NotFound", "Token de activación no encontrado.");

        if (!token.IsValid())
            return Error.Validation("ActivationToken.Invalid", "El token ha expirado o ya fue usado.");

        var user = await _userRepository.GetByIdAsync(new UserId(request.UserId));
        if (user is null)
            return Error.NotFound("User.NotFound", "Usuario no encontrado.");

        if (user.Active)
            return Error.Conflict("User.AlreadyActive", "El usuario ya fue activado.");

        var passwordHash = _passwordHasher.HashPassword(request.Password);
        user.Activate(passwordHash);

        token.MarkAsUsed();
        _tokenRepository.Update(token, cancellationToken);

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id.Value;
    }
}
