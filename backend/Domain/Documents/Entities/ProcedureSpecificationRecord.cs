using Domain.Partners.Entities;
using Domain.Security.Entities;

namespace Domain.Documents.Entities;

public class ProcedureSpecificationRecord : ProcedureSpecification
{
    public ProcedureSpecificationRecord(
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
        : base(id, name, path, uploadDate, expirationDate, description, uploadedBy, assignedTo, procedureNumber, standardCode)
    {
        DocumentType = Enums.DocumentType.ProcedureSpecificationRecord;
    }

    public ProcedureSpecificationRecord() { }
}