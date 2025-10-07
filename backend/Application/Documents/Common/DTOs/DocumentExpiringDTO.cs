namespace Application.Documents.Common.DTOs;
public class DocumentExpiringDTO
{
    public Guid DocumentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; }
    public List<string> AssignedToEmails { get; set; } = new();
}
