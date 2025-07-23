using Azure.Core;
using Domain.Primitives;
using Domain.Security.Entities;
using Domain.Security.Interfaces;
using Domain.Secutiry.Interfaces;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.Edit;
public class EditUserCommandHandler : IRequestHandler<EditUserCommand, ErrorOr<UserId>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditUserCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

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

        user.Update(command.Name, emailResult, role);

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
