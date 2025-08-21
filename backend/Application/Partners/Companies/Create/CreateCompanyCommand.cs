using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Partners.Companies.Create;
public record CreateCompanyCommand(
    string Name,
    Cuit Cuit
) : IRequest<ErrorOr<Guid>>;

