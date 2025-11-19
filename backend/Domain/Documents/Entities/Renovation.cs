using Domain.Partners.Entities;
using Domain.Security.Entities;

namespace Domain.Documents.Entities;

public class Renovation : Certificate
{
    public Renovation(
        DocumentFileId id,
        string name,
        string path,
        DateTime uploadDate,
        DateTime? expirationDate,
        string description,
        User uploadedBy,
        Company? assignedTo,
        DateTime validFrom,
        string certificateNumber,
        string employerName,
        string code
    ) : base(id, name, path, uploadDate, expirationDate, description, uploadedBy, assignedTo, validFrom, certificateNumber, employerName, code)
    {
    }

    public Renovation() : base() { }
}