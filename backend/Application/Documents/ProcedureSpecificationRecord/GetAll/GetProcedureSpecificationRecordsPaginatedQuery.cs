using Application.Common.Responses;
using Application.Documents.ProcedureSpecificationRecord.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Documents.ProcedureSpecificationRecord.GetAll;

public record GetProcedureSpecificationRecordsPaginatedQuery(int Page, int PageSize, string? Filter) : IRequest<ErrorOr<PaginatedResult<ProcedureSpecificationRecordResponseDTO>>>;