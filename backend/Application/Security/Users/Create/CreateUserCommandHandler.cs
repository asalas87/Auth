using Application.Interfaces;
using Domain.Partners.Interfaces;
using Domain.Primitives;
using Domain.Security.Entities;
using Domain.Security.Interfaces;
using Domain.Secutiry.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.Create
{
    public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, ICompanyRepository companyRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {

            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<ErrorOr<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                return Error.Failure("User.EmailRegistrated", "El email ya está registrado.");

            var existingRole = await _roleRepository.GetByIdAsync(request.RoleId);
            if (existingRole == null)
                return Error.Failure("Role.RoleNoExist", "No existe rol.");

            var user = new User(request.Name, _passwordHasher.HashPassword(request.Password), request.Email, existingRole, true);
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return user.Id.Value;
        }
    }
}
