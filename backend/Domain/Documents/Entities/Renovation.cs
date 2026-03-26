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
    }
    public int RenovationNumber { get; private set; }

    public Renovation() : base() { }    
}
