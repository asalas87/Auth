using Domain.Partners.Entities;
using Domain.Security.Entities;

namespace Domain.Documents.Entities;
public class Certificate : DocumentFile
{
    public Certificate(
        DocumentFileId id,
        string name,
        string path,
        DateTime uploadDate,
        DateTime? expirationDate,
        string description,
        User uploadedBy,
        Company? assignedTo,
        DateTime validFrom,
        DateTime validUntil
    ) : base(id, name, path, uploadDate, expirationDate, description, uploadedBy, assignedTo, Enums.DocumentType.Certificate)
    {
        ValidFrom = validFrom;
        ValidUntil = validUntil;
    }

    public DateTime ValidFrom { get; private set; }
    public DateTime ValidUntil { get; private set; }

    public Certificate() { }
}


