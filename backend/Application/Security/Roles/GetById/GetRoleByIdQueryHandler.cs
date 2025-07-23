using Application.Security.Common.DTOS;
using Application.Security.Roles.GetById;
using Domain.Primitives;
using Domain.Security.Entities;
using Domain.Security.Interfaces;
using Domain.Secutiry.Interfaces;
using ErrorOr;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Security.Roles.GetAll;

public class GetRolesByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, ErrorOr<Role>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoleRepository _roleRepository;

    public GetRolesByIdQueryHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    public async Task<ErrorOr<Role>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        if (await _roleRepository.GetByIdAsync(request.Id) is not Role role)
        {
            return Error.NotFound("Role.NotFound", "The role with the provide Id was not found.");
        }
        return role;
    }
}
