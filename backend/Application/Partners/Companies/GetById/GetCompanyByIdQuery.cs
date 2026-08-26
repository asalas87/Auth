using Application.Partners.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Partners.Companies.GetById;

public record GetCompanyByIdQuery(Guid Id) : IRequest<ErrorOr<CompanyDto>>;
