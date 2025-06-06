using Application.Common.Dtos;
using Microsoft.AspNetCore.Http;

namespace Application.Documents.Management.DTOs;

public class CreateDocumentDTO : FileDTO
{
    public string? Description { get; set; } = string.Empty;
    public Guid UploadedBy { get; set; }
    public Guid? AssignedTo { get; set; }
    public bool? IsRead { get; set; }
    public IFormFile File { get; set; } = default!;
}
