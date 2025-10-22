using Domain.Primitives;
using Domain.Security.Entities;
using Domain.Security.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Security.Roles.GetById;

public class GetRolesByIdQueryHandler(IRoleRepository roleRepository) : IRequestHandler<GetRoleByIdQuery, ErrorOr<Role>>
{
    private readonly IRoleRepository _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));

    public async Task<ErrorOr<Role>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        if (await _roleRepository.GetByIdAsync(request.Id) is not Role role)
        {
            return Error.NotFound("Role.NotFound", "The role with the provide Id was not found.");
        }
        return role;
    }
}
