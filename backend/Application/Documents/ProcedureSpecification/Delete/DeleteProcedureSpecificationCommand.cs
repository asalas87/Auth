using ErrorOr;
using MediatR;

namespace Application.Documents.ProcedureSpecification.Delete;

public record DeleteProcedureSpecificationCommand(Guid Id) : IRequest<ErrorOr<Guid>>;