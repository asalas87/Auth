using ErrorOr;
using MediatR;

namespace Application.Partners.Companies.Delete;

public record DeleteCompanyCommand(Guid Id) : IRequest<ErrorOr<Unit>>;
