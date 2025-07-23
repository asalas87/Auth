using Application.Security.Common.DTOS;
using Domain.Primitives;
using Domain.Security.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Security.Roles.GetAll;

public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, ErrorOr<List<RoleDTO>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoleRepository _roleRepository;

    public GetAllRolesQueryHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    public async Task<ErrorOr<List<RoleDTO>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var users = await _roleRepository.GetAll();

        var items = users.Select(r => new RoleDTO
        {
            Id = r.Id,
            RoleName = r.Name,
        }).ToList();

        return items;
    }
}
