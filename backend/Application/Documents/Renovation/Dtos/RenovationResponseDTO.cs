using Application.Documents.Renovation.Dtos;

namespace Application.Documents.Renovation.DTOs;

public class RenovationResponseDTO : RenovationEditDTO
{
    public string UploadedBy { get; set; } = string.Empty;
}
