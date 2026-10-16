using Application.Common.Responses;
using Application.Partners.Dtos;
using Domain.Partners.Entities;
using Domain.Partners.Interfaces;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Partners.Companies.GetAll;

public sealed class GetCompaniesPagedQueryHandler(ICompanyRepository companyRepository) : IRequestHandler<GetCompaniesPagedQuery, ErrorOr<PaginatedResult<CompanyDto>>>
{
    public async Task<ErrorOr<PaginatedResult<CompanyDto>>> Handle(GetCompaniesPagedQuery request, CancellationToken cancellationToken)
    {
        var (companies, totalCount) = await companyRepository.GetPaginatedAsync(request.Page, request.PageSize, request.Filter, cancellationToken);

        var items = companies.Select(c => new CompanyDto
        {
            Id = c.Id.Value,
            Name = c.Name,
            CuitCuil = c.CuitCuil?.ToString()
        }).ToList();

        return new PaginatedResult<CompanyDto>
        {
            Items = items,
            TotalCount = totalCount
        };
    }
}
