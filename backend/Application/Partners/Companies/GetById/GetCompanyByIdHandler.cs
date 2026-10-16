using Application.Partners.Dtos;
using Domain.Partners.Entities;
using Domain.Partners.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Partners.Companies.GetById;

public sealed class GetCompanyByIdHandler(ICompanyRepository companyRepository) : IRequestHandler<GetCompanyByIdQuery, ErrorOr<CompanyDto>>
{
    public async Task<ErrorOr<CompanyDto>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetByIdAsync(new CompanyId(request.Id), cancellationToken);

        if (company is null)
            return Error.NotFound("Company.NotFound", "La empresa no existe.");

        return new CompanyDto
        {
            Id = company.Id.Value,
            Name = company.Name,
            CuitCuil = company.CuitCuil?.ToString()
        };
    }
}
