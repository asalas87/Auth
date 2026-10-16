using Application.Documents.Management.DTOs;

namespace Application.Documents.ProcedureSpecification.Dtos;

public class ProcedureSpecificationDTO : DocumentEditDTO
{
    public string ProcedureNumber { get; set; } = string.Empty;
    public string StandardCode { get; set; } = string.Empty;
}