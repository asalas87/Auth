namespace Application.Documents.Certificate.Dtos;

public class CertificateResponseDTO : CertificateEditDTO
{
    public string UploadedBy { get; set; } = string.Empty;
}
