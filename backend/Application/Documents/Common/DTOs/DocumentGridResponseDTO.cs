namespace Application.Documents.Common.DTOs;
public class DocumentGridResponseDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CertificateNumber { get; set; } = string.Empty;
    public string EmployerFullName { get; set; } = string.Empty;
    public string StandardCode { get; set; } = string.Empty;
    public DateTime? Validity { get; set; }
    public string Type {  get; set; } = string.Empty;
    public bool IsRead { get; set; }
}
