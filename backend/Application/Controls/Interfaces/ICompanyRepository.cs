using Domain.Partners.Entities;
using Domain.ValueObjects;

public interface ICompanyRepository
{
    Task<List<Company>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Company?> GetByCuitAsync(Cuit cuit, CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(Company company, CancellationToken cancellationToken = default);
}
