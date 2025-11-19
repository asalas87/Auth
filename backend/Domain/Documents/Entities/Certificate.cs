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
        string certificateNumber,
        string employerName,
        string code
    ) : base(name, path, uploadDate, expirationDate, description, uploadedBy, assignedTo, Enums.DocumentType.Certificate)
    {
        ValidFrom = validFrom;
        CertificateNumber = certificateNumber;
        EmployerName = employerName;
        Code = code;
    }

    public string CertificateNumber { get; private set; } = string.Empty;
    public string EmployerName { get; private set; } = string.Empty;
    public DateTime ValidFrom { get; private set; }
    public string Code { get; private set; } = string.Empty;

    public Certificate() { }
    public void Update(
        string certificateNumber,
        string employerName,
        string code,
        DateTime validFrom,
        DateTime expirationDate,
        Company assignedTo)
    {
        CertificateNumber = certificateNumber;
        EmployerName = employerName;
        AssignedTo = assignedTo;
        ValidFrom = validFrom;
        ExpirationDate = expirationDate;
        Code = code;
    }
}
