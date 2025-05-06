using Application.Documents.Management.DTOs;
using Application.Documents.Management.Services;
using ErrorOr;

namespace Infrastructure.Documents.Services;

public class DocumentService : IDocumentService
{
    public async Task<ErrorOr<Success>> UploadDocumentAsync(DocumentUploadDTO dto)
    {
        // Aquí podés hacer la lógica de guardado del archivo + persistencia
        // Por ahora, simulemos éxito
        return Result.Success;
    }
}
