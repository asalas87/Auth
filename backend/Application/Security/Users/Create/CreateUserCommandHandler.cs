using Domain.Partners.Interfaces;
using Domain.Primitives;
using Domain.Security.Entities;
using Domain.Security.Events;
using Domain.Security.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.Create;

public sealed class CreateUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, ICompanyRepository companyRepository, IUnitOfWork unitOfWork, IPublisher publisher) : IRequestHandler<CreateUserCommand, ErrorOr<Guid>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IPublisher _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly IRoleRepository _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
    private readonly ICompanyRepository _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));

    public async Task<ErrorOr<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
            return Error.Failure("User.EmailRegistrated", "El email ya está registrado.");

        var existingRole = await _roleRepository.GetByIdAsync(request.RoleId);
        if (existingRole == null)
            return Error.Failure("Role.RoleNoExist", "No existe rol.");

        var existingCompany = await _companyRepository.GetByIdAsync(request.CompanyId, cancellationToken);
        if (existingCompany == null)
            return Error.Failure("Company.CompanyNotExist", "No existe la empresa.");

        var user = User.Create(request.Name, request.Email, existingRole, existingCompany);
        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var userCreatedEvent = new UserCreatedEvent(user.Id.Value, user.Name, user.Email.Value, request.CompanyId.Value);
        await _publisher.Publish(userCreatedEvent, cancellationToken);

        return user.Id.Value;
    }
}
