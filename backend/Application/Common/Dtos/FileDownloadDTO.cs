namespace Application.Common.Dtos;
public class FileDownloadDTO
{
    public byte[] Content { get; set; } = default!;
    public string FileName { get; set; } = "document.pdf";
    public string ContentType { get; set; } = "application/pdf";
}
