using ErrorOr;
using MediatR;

namespace Application.Documents.Renovation.Delete;

public record DeleteRenovationCommand(Guid Id) : IRequest<ErrorOr<Guid>>;
