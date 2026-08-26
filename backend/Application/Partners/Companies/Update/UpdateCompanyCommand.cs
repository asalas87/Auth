using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Partners.Companies.Update;

public record UpdateCompanyCommand(
    Guid Id,
    string Name,
    Cuit Cuit
) : IRequest<ErrorOr<Guid>>;
