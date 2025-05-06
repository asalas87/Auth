using Application.Documents.Management.DTOs;
using ErrorOr;

namespace Application.Documents.Management.Services;

public interface IDocumentService
{
    Task<ErrorOr<Success>> UploadDocumentAsync(DocumentUploadDTO dto);
}
