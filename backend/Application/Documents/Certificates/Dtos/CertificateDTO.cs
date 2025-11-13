using Application.Documents.Management.DTOs;

namespace Application.Documents.Certificate.DTOs;

public class CertificateDTO : DocumentEditDTO
{
    public string AssignedTo { get; set; } = string.Empty;
    public DateTime ValidFrom { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public string EmployerName { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
