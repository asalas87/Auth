using Application.Common.Responses;
using Application.Documents.ProcedureSpecification.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Documents.ProcedureSpecification.GetAll;

public record GetProcedureSpecificationsPaginatedQuery(int Page, int PageSize, string? Filter) : IRequest<ErrorOr<PaginatedResult<ProcedureSpecificationResponseDTO>>>;