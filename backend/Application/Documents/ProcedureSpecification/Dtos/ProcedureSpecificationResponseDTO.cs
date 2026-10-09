namespace Application.Documents.ProcedureSpecification.Dtos;

public class ProcedureSpecificationResponseDTO : ProcedureSpecificationEditDTO
{
    public string UploadedBy { get; set; } = string.Empty;
    public string AssignedTo { get; set; } = string.Empty;
}