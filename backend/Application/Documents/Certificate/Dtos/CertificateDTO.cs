using Application.Documents.Management.DTOs;

namespace Application.Documents.Certificate.Dtos;

public class CertificateDTO : DocumentEditDTO
{
    public string AssignedTo { get; set; } = string.Empty;
    public DateTime Validity { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public string EmployerFullName { get; set; } = string.Empty;
    public string StandardCode { get; set; } = string.Empty;
}
