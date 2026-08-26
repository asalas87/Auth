using Application.Common.Responses;
using Application.Partners.Companies.Create;
using Application.Partners.Companies.Delete;
using Application.Partners.Companies.GetAll;
using Application.Partners.Companies.GetById;
using Application.Partners.Companies.Update;
using Application.Partners.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Partners.Services;

public class CompanyService(ISender mediator) : ICompanyService
{
    private readonly ISender _mediator = mediator;

    public async Task<ErrorOr<PaginatedResult<CompanyDto>>> GetPagedAsync(CompanyFilterDto filter)
    {
        var query = new GetCompaniesPagedQuery(filter.Page, filter.PageSize, filter.Filter);
        return await _mediator.Send(query);
    }

    public async Task<ErrorOr<CompanyDto>> GetByIdAsync(Guid id)
    {
        var query = new GetCompanyByIdQuery(id);
        return await _mediator.Send(query);
    }

    public async Task<ErrorOr<SuccessResponse>> CreateAsync(string name, string cuit)
    {
        var cuitResult = Domain.ValueObjects.Cuit.Create(cuit);
        if (cuitResult == null)
            return Error.Failure("Company.InvalidCuit", "El CUIT es inválido.");

        var command = new CreateCompanyCommand(name, cuitResult);
        var result = await _mediator.Send(command);

        if (result.IsError)
            return result.Errors;

        return new SuccessResponse("Empresa creada correctamente.", result.Value);
    }

    public async Task<ErrorOr<SuccessResponse>> UpdateAsync(Guid id, string name, string cuit)
    {
        var cuitResult = Domain.ValueObjects.Cuit.Create(cuit);
        if (cuitResult == null)
            return Error.Failure("Company.InvalidCuit", "El CUIT es inválido.");

        var command = new UpdateCompanyCommand(id, name, cuitResult);
        var result = await _mediator.Send(command);

        if (result.IsError)
            return result.Errors;

        return new SuccessResponse("Empresa actualizada correctamente.", id);
    }

    public async Task<ErrorOr<SuccessResponse>> DeleteAsync(Guid id)
    {
        var result = await _mediator.Send(new DeleteCompanyCommand(id));

        if (result.IsError)
            return result.Errors;

        return new SuccessResponse("Empresa eliminada correctamente.", id);
    }
}
