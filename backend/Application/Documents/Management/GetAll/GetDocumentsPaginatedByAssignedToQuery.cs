using Application.Common.Responses;
using Application.Documents.Management.DTOs;
using ErrorOr;
using MediatR;

namespace Application.Documents.Management.GetAll
{
    public record GetDocumentsPaginatedByAssignedToQuery(int Page, int PageSize, string? Filter, Guid AssignedToUserId) : IRequest<ErrorOr<PaginatedResult<DocumentResponseDTO>>>;
}
