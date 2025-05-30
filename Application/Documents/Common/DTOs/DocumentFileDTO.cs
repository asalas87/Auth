using Application.Common.Dtos;

namespace Application.Documents.Management.DTOs;

public class DocumentFileDTO : FileDTO
{
    public string? Description { get; set; } = string.Empty;
    public string UploadedBy { get; set; } = string.Empty;
    public string? AssignedTo {  get; set; } = string.Empty;
    public bool ? IsRead { get; set; }

}
