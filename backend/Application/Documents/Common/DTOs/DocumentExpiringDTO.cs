namespace Application.Documents.Common.DTOs;
public class ExpiringDocumentDTO
{
    public Guid DocumentId { get; set; }
    public Guid CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; }
    public List<string> AssignedToEmails { get; set; } = new();
    public List<string> AssignedToNames { get; set; } = new();
}
