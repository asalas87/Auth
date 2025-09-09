using Domain.Partners.Entities;
using Domain.Security.Entities;

namespace Domain.Documents.Entities;
public class Certificate : DocumentFile
{
    public Certificate(
        string name,
        string path,
        DateTime uploadDate,
        DateTime? expirationDate,
        string description,
        User uploadedBy,
        Company? assignedTo,
        DateTime validFrom,
        DateTime validUntil
    ) : base(name, path, uploadDate, expirationDate, description, uploadedBy, assignedTo, Enums.DocumentType.Certificate)
    {
        ValidFrom = validFrom;
        ValidUntil = validUntil;
    }

    public DateTime ValidFrom { get; private set; }
    public DateTime ValidUntil { get; private set; }

    public Certificate() { }
    public void Update(
        DateTime validFrom,
        DateTime validUntil,
        Company assignedTo)
    {
        AssignedTo = assignedTo;
        ValidFrom = validFrom;
        ValidUntil = validUntil;
    }
}
