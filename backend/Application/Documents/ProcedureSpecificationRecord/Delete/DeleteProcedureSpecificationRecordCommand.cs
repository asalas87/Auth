using ErrorOr;
using MediatR;

namespace Application.Documents.ProcedureSpecificationRecord.Delete;

public record DeleteProcedureSpecificationRecordCommand(Guid Id) : IRequest<ErrorOr<Guid>>;