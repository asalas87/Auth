using Application.Controls.Interfaces;
using Domain.Partners.Entities;
using Domain.Partners.Interfaces;
using Domain.Primitives;
using Domain.Security.Entities;
using Domain.Security.Interfaces;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.Edit;
public class EditUserCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<EditUserCommand, ErrorOr<UserId>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly ICompanyRepository _companyRepository = companyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<UserId>> Handle(EditUserCommand command, CancellationToken cancellationToken)
    {
        var userId = new UserId(command.Id);
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
            return Error.NotFound("User.NotFound", "El usuario no existe.");

        var emailResult = Email.Create(command.Email);
        if (emailResult == null)
            return Error.Failure("Email inválido");

        var role = await _roleRepository.GetByIdAsync(command.RoleId);

        if (role is null)
            return Error.NotFound("Role.NotFound", "El rol no existe.");

        var company = await _companyRepository.GetByIdAsync(new CompanyId(command.CompanyId), cancellationToken);
        if (company is null)
            return Error.NotFound("Company.NotFound", "La empresa no existe.");

        user.Update(command.Name, emailResult, role, company);

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
