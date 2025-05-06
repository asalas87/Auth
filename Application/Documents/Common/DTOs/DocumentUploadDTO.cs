using Microsoft.AspNetCore.Http;

namespace Application.Documents.Management.DTOs;

public class DocumentUploadDTO
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; }
    public Guid UploadedBy { get; set; } 
    public Guid AssignedTo { get; set; }
    public IFormFile File { get; set; } = default!;
}
