using Domain.Primitives;
using Domain.Security.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.Delete
{
    public sealed class DeleteUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserCommand, ErrorOr<Unit>>
    {
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        public async Task<ErrorOr<Unit>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user is null)
                return Error.NotFound("User.NotFound", "Usuario no encontrado.");

            _userRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
