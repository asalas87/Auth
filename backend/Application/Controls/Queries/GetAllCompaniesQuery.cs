using Domain.Partners.Entities;
using ErrorOr;
using MediatR;

namespace Application.Controls.Queries;

public record GetAllCompaniesQuery() : IRequest<ErrorOr<List<Company>>>;
