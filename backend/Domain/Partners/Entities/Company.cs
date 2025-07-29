using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Partners.Entities;
public sealed class Company : AggergateRoot<CompanyId>
{
    public Company(CompanyId id, string name, Cuit cuitCuil, bool active)
    {
        Id = id;
        Name = name;
        CuitCuil = cuitCuil;
    }
    public Company()
    {
    }
    public string Name { get; private set; } = string.Empty;
    public Cuit CuitCuil { get; private set; } = Cuit.Create(string.Empty);
    public bool IsActive { get; set; }
    public static Company UpdateCompany(Guid id, string name, Cuit cuitCuil, bool active)
    {
        return new Company(new CompanyId(id), name, cuitCuil, active);
    }
}
