using Domain.Primitives;
using ErrorOr;
using MediatR;
using Domain.Security.Entities;
using Application.Interfaces;
using Domain.Secutiry.Interfaces;
using Domain.Security.Interfaces;

namespace Application.Security.Users.Create
{
    public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {

            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<ErrorOr<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                return Error.Failure("User.EmailRegistrated", "El email ya está registrado.");
            var role = await _roleRepository.GetByIdAsync(request.RoleId);

            var user = new User(request.Name, _passwordHasher.HashPassword(request.Password), request.Email, role, true);
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return user.Id.Value;
        }
    }
}
