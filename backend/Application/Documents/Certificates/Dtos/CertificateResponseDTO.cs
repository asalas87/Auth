namespace Application.Documents.Certificate.DTOs;

public class CertificateResponseDTO : CertificateEditDTO
{
    public string UploadedBy { get; set; } = string.Empty;
    public DateTime uploadedDate { get; set; }
}
