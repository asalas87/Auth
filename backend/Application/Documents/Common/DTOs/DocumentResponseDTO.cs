namespace Application.Documents.Management.DTOs;

public class DocumentResponseDTO : DocumentEditDTO
{
    public string AssignedTo {  get; set; } = string.Empty;
    public string UploadedBy {  get; set; } = string.Empty;    
}
