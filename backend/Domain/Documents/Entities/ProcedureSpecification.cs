using Domain.Partners.Entities;
using Domain.Security.Entities;

namespace Domain.Documents.Entities;

public class ProcedureSpecification : DocumentFile
{
    public ProcedureSpecification(
        DocumentFileId id,
        string name,
        string path,
        DateTime uploadDate,
        DateTime? expirationDate,
        string description,
        User uploadedBy,
        Company? assignedTo,
        string procedureNumber,
        string standardCode)
        : base(name, path, uploadDate, expirationDate, description, uploadedBy, assignedTo, Enums.DocumentType.WeldingProcedure)
    {
        Id = id;
        ProcedureNumber = procedureNumber;
        StandardCode = standardCode;
    }

    public ProcedureSpecification() { }

    public string ProcedureNumber { get; protected set; } = string.Empty;
    public string StandardCode { get; protected set; } = string.Empty;

    public void Update(
        Company? assignedTo,
        string procedureNumber,
        string standardCode)
    {
        AssignedTo = assignedTo;
        ProcedureNumber = procedureNumber;
        StandardCode = standardCode;
    }

    public void ReplaceFile(string relativePath)
    {
        RelativePath = relativePath;
    }
}
