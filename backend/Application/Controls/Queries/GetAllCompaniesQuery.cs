using Application.Controls.Dtos;
using MediatR;

namespace Application.Controls.Queries;

public record GetAllCompaniesQuery() : IRequest<List<CompanyLookupDto>>;
