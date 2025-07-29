using Domain.Partners.Entities;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Controls.Queries;

public record GetCompanyByCuitQuery(Cuit cuit) : IRequest<ErrorOr<Company>>;
