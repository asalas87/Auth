using Application.Documents.ProcedureSpecificationRecord.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Documents.ProcedureSpecificationRecord.GetById;

public record GetProcedureSpecificationRecordByIdQuery(Guid Id) : IRequest<ErrorOr<ProcedureSpecificationRecordResponseDTO>>;