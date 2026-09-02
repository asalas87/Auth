using Domain.Primitives;
using Domain.Security.Events;
using Domain.Security.Interfaces;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.ResetPassword;

public sealed class RequestPasswordResetCommandHandler(
    IUserRepository userRepository,
    IPublisher publisher
) : IRequestHandler<RequestPasswordResetCommand, ErrorOr<bool>>
{
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly IPublisher _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));

    public async Task<ErrorOr<bool>> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);
        if (emailResult is null)
            return Error.Validation("Email.Invalid", "El email no es válido.");

        var user = await _userRepository.GetByEmailAsync(emailResult);

        if (user is null)
            return true;

        var passwordResetEvent = new PasswordResetRequestedEvent(
            user.Id.Value,
            user.Name,
            user.Email.Value,
            user.Company.Id.Value
        );

        await _publisher.Publish(passwordResetEvent, cancellationToken);

        return true;
    }
}
