using Application.Documents.ProcedureSpecification.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Documents.ProcedureSpecification.GetById;

public record GetProcedureSpecificationByIdQuery(Guid Id) : IRequest<ErrorOr<ProcedureSpecificationResponseDTO>>;