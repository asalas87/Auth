using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Security.Common.DTOS;
using Application.Security.Roles.GetAll;
using AutoMapper;
using ErrorOr;
using MediatR;

namespace Application.Security.Services;

public class RoleService : IRoleService
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public RoleService(IMapper mapper, ISender mediator)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<ErrorOr<List<RoleDTO>>> GetRolesAsync()
    {
        return await _mediator.Send(new GetAllRolesQuery()).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<List<RoleDTO>>>(result);
        });
    }
}
