using Domain.Primitives;
using ErrorOr;
using MediatR;
using Domain.Secutiry.Interfaces;
using Domain.Secutiry.Entities;
using Application.Interfaces;

namespace Application.Security.Users.Create
{
    public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {

            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<ErrorOr<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                return Error.Failure("User.EmailRegistrated", "El email ya está registrado.");

            var user = new User(new UserId(Guid.NewGuid()), request.Name, _passwordHasher.HashPassword(request.Password), request.Email, true);
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return user.Id.Value;
        }
    }
}
