namespace Application.Common.Dtos;
public class ResponseFileDTO : FileEditDTO
{
    public string UploadedBy { get; set; } = string.Empty;
}
