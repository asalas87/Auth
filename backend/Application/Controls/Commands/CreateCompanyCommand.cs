using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Controls.Commands;

public record CreateCompanyCommand(string Name, Cuit Cuit) : IRequest<ErrorOr<Guid>>;
