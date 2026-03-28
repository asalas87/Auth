using Application.Common.Dtos;
using Application.Common.Responses;
using Application.Documents.Certificate.Dtos;
using Application.Documents.Common.DTOs;
using Application.Documents.Management.DTOs;
using Application.Documents.Renovation.Dtos;
using Application.Documents.Renovation.DTOs;
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
    Task<ErrorOr<CertificateResponseDTO>> GetCertificateByIdAsync(Guid id);
    Task<ErrorOr<FileDownloadDTO>> DownloadAsync(Guid id);
    Task<ErrorOr<PaginatedResult<RenovationResponseDTO>>> GetRenovationsPaginatedAsync(PaginateDTO paginateDTO);
    Task<ErrorOr<Guid>> CreateRenovationAsync(RenovationDTO dto);
    Task<ErrorOr<Guid>> UpdateRenovationAsync(RenovationEditDTO dto);
    Task<ErrorOr<Guid>> DeleteRenovationAsync(Guid id);
    Task<ErrorOr<Guid>> DeleteDocumentAsync(Guid id);
    Task<ErrorOr<FileDownloadDTO>> DownloadMultipleAsync(List<Guid> ids);
    Task<ErrorOr<List<DocumentGridResponseDTO>>> GetUserDocumentsAsync();
}
