namespace Application.Documents.Management.DTOs;

public class DocumentFileDTO
{
    public Guid Id { get; set; }
    public string Name { get;  set; } = string.Empty;
    public DateTime UploadDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
}
