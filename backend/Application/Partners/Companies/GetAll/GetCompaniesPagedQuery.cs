using Application.Common.Responses;
using Application.Partners.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Partners.Companies.GetAll;

public record GetCompaniesPagedQuery(int Page, int PageSize, string? Filter) : IRequest<ErrorOr<PaginatedResult<CompanyDto>>>;
