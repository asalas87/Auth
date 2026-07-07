using Application.Controls.Dtos;
using Application.Controls.Queries;
using Application.Security.Common.DTOS;
using Application.Security.Roles.GetAll;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Controls.Services;
public class ControlService : IControlService
{
    private readonly ISender _mediator;

    public ControlService(ISender mediator)
    {
        _mediator = mediator;
    }

    public Task<ErrorOr<Guid>> CreateCompanyAsync(CreateCompanyDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<ErrorOr<List<CompanyLookupDto>>> GetAllCompaniesAsync(CancellationToken cancellationToken)
    {
        var companies = await _mediator.Send(new GetAllCompaniesQuery(), cancellationToken);
        if (companies.IsError)
            return companies.Errors;

        return companies.Value.Select(c => new CompanyLookupDto(c.Id.Value, c.Name)).ToList();
    }

    public async Task<ErrorOr<CompanyLookupDto>> GetCompanyByCuitAsync(string cuit, CancellationToken cancellationToken = default)
    {
        var query = new GetCompanyByCuitQuery(Cuit.Create(cuit));
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsError)
            return result.Errors;

       return new CompanyLookupDto(result.Value.Id.Value, result.Value.Name + "-" + result.Value.CuitCuil.ToString());
    }

    public async Task<ErrorOr<List<RoleDTO>>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        var roles = await _mediator.Send(new GetAllRolesQuery(), cancellationToken);
        if (roles.IsError)
            return roles.Errors;

        return roles.Value.Select(c => new RoleDTO{ Id = c.Id, RoleName = c.RoleName }).ToList();
    }
}
