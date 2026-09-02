using Application.Interfaces;
using Domain.Primitives;
using Domain.Security.Entities;
using Domain.Security.Enums;
using Domain.Security.Interfaces;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.ResetPassword;

public sealed class ResetPasswordCommandHandler(
    IUserRepository userRepository,
    IUserActivationTokenRepository tokenRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork
) : IRequestHandler<ResetPasswordCommand, ErrorOr<bool>>
{
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly IUserActivationTokenRepository _tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
    private readonly IPasswordHasher _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<ErrorOr<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);
        if (emailResult is null)
            return Error.Validation("Email.Invalid", "El email no es válido.");

        var token = await _tokenRepository.GetByTokenAsync(request.Token, cancellationToken);
        if (token is null || !token.IsValid() || token.Purpose != TokenPurpose.PasswordReset)
            return Error.Validation("PasswordReset.Token.Invalid", "El token no es válido o ha expirado.");

        var user = await _userRepository.GetByEmailAsync(emailResult);
        if (user is null)
            return Error.NotFound("User.NotFound", "Usuario no encontrado.");

        if (!user.Id.Equals(token.UserId))
            return Error.Validation("PasswordReset.Token.Invalid", "El token no corresponde a este usuario.");

        var passwordHash = _passwordHasher.HashPassword(request.Password);
        user.Password = passwordHash;

        token.MarkAsUsed();

        await _tokenRepository.InvalidateOtherTokensAsync(user.Id, token.Token, cancellationToken);

        _userRepository.Update(user);
        _tokenRepository.Update(token, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
