using Domain.Partners.Entities;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Partners.Companies.GetById;
public record GetCompanyByCuitQuery(
    Cuit Cuit
) : IRequest<ErrorOr<Company>>;
