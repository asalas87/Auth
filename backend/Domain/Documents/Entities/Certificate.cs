using Domain.Partners.Entities;
using Domain.Security.Entities;

namespace Domain.Documents.Entities;
public class Certificate : DocumentFile
{
    public Certificate(
        string name,
        string path,
        DateTime uploadDate,
        string description,
        User uploadedBy,
        Company? assignedTo,
        DateTime validity,
        string certificateNumber,
        string employerFullName,
        string standardCode
    ) : base(name, path, uploadDate, validity, description, uploadedBy, assignedTo, Enums.DocumentType.Qualification)
    {
        Validity = validity;
        ExpirationDate = validity;
        CertificateNumber = certificateNumber;
        EmployerFullName = employerFullName;
        StandardCode = standardCode;
    }

    public string CertificateNumber { get; protected set; } = string.Empty;
    public string EmployerFullName { get; protected set; } = string.Empty;
    public DateTime Validity { get; protected set; }
    public string StandardCode { get; protected set; } = string.Empty;

    public Certificate() { }
    public void Update(
        string certificateNumber,
        string employerFullName,
        string standardCode,
        DateTime validity,
        Company assignedTo)
    {
        CertificateNumber = certificateNumber;
        EmployerFullName = employerFullName;
        AssignedTo = assignedTo;
        Validity = validity;
        ExpirationDate = validity;
        StandardCode = standardCode;
    }
}
