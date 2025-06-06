using Application.Common.Dtos;
using Application.Common.Responses;
using Application.Documents.Common.DTOs;
using Application.Documents.Management.DTOs;
using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace Application.Documents.Services;

public interface IDocumentService
{
    Task<ErrorOr<PaginatedResult<DocumentFileDTO>>> GetDocumentsPaginatedAsync(PaginateDTO paginateDTO);
    Task<ErrorOr<PaginatedResult<DocumentFileDTO>>> GetDocumentsAsignedToPaginatedAsync(DocumentAssignedDTO documentAssignedDTO);
    Task<ErrorOr<Guid>> CreateDocumentAsync(CreateDocumentDTO dto);
}
