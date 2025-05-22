using Microsoft.AspNetCore.Http;

namespace Application.Documents.Management.DTOs;

public class CreateDocumentDTO : DocumentFileDTO
{
    public Guid? AssignedTo { get; set; }
    public IFormFile File { get; set; }
}
