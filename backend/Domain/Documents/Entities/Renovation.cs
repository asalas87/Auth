using Domain.Partners.Entities;
using Domain.Security.Entities;

namespace Domain.Documents.Entities;

public class Renovation : Certificate
{
    public Renovation(
        string name,
        string path,
        DateTime uploadDate,
        string description,
        User uploadedBy,
        Company? assignedTo,
        DateTime validity,
        string certificateNumber,
        string employerFullName,
        string standardCode,
        int renovationNumber

    ) : base(name, path, uploadDate, description, uploadedBy, assignedTo, validity, certificateNumber, employerFullName, standardCode)
    {
        RenovationNumber = renovationNumber;
        DocumentType = Enums.DocumentType.Renovation;
    }
    public int RenovationNumber { get; private set; }

    public Renovation() : base() { }
    public void Update(
        string certificateNumber,
        int renovationNumber,
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
        RenovationNumber = renovationNumber;
    }
}
