using Application.Common.Responses;
using Application.Documents.Management.DTOs;
using ErrorOr;
using MediatR;

namespace Application.Documents.Management.GetAll
{
    public record GetDocumentsPaginatedQuery(int Page, int PageSize, string? Filter) : IRequest<ErrorOr<PaginatedResult<DocumentFileDTO>>>;
}
