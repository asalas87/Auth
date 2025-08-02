using Application.Documents.Management.DTOs;

namespace Application.Documents.Certificate.DTOs;

public class CertificateDTO : DocumentEditDTO
{
    public string AssignedTo { get; set; } = string.Empty;
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
}
