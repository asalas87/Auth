using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Partners.Entities;
public sealed class Company : AggergateRoot<CompanyId>
{
    public Company(string name, Cuit cuitCuil)
    {
        Name = name;
        CuitCuil = cuitCuil;
        IsActive = true;
    }
    public Company() { }
    public string Name { get; private set; } = string.Empty;
    public Cuit CuitCuil { get; private set; } 
    public bool IsActive { get; private set; }
    public void UpdateCompany(string name, Cuit cuitCuil, bool active)
    {
        Name = name;
        CuitCuil = cuitCuil;
        IsActive = active;
    }
}
