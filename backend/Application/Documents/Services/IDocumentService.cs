using Application.Common.Dtos;
using Application.Common.Responses;
using Application.Documents.Certificate.DTOs;
using Application.Documents.Common.DTOs;
using Application.Documents.Management.DTOs;
using ErrorOr;

namespace Application.Documents.Services;

public interface IDocumentService
{
    Task<ErrorOr<PaginatedResult<DocumentResponseDTO>>> GetDocumentsPaginatedAsync(PaginateDTO paginateDTO);
    Task<ErrorOr<PaginatedResult<DocumentResponseDTO>>> GetDocumentsAsignedToPaginatedAsync(DocumentAssignedDTO documentAssignedDTO);
    Task<ErrorOr<Guid>> CreateDocumentAsync(DocumentDTO dto);
    Task<ErrorOr<PaginatedResult<CertificateResponseDTO>>> GetCertificatesPaginatedAsync(PaginateDTO paginateDTO);
    Task<ErrorOr<Guid>> CreateCertificateAsync(CertificateDTO dto);
    Task<ErrorOr<Guid>> UpdateCertificateAsync(CertificateEditDTO dto);
    Task<ErrorOr<Guid>> DeleteCertificateAsync(Guid id);
}
