using Application.Common.Dtos;

namespace Application.Documents.Management.DTOs;

public class DocumentDTO : FileEditDTO
{
    public string? Description { get; set; } = string.Empty;
    public Guid? AssignedToId { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool? IsRead { get; set; }
}
