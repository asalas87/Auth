using Domain.Partners.Entities;
using Domain.Security.Entities;

namespace Domain.Documents.Entities;
public class GeneralDocument : DocumentFile
{
    public GeneralDocument(
        DocumentFileId id,
        string name,
        string path,
        DateTime uploadDate,
        DateTime? expirationDate,
        string description,
        User uploadedBy,
        Company? assignedTo
    ) : base(id, name, path, uploadDate, expirationDate, description, uploadedBy, assignedTo, Enums.DocumentType.General)
    {
    }

    public GeneralDocument() { }
}
