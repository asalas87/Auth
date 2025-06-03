using Microsoft.AspNetCore.Http;

namespace Application.Documents.Management.DTOs;

public class CreateDocumentDTO : DocumentFileDTO
{
    public IFormFile File { get; set; } = default!;
}
