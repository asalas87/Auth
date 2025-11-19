using Domain.Partners.Entities;
using Domain.ValueObjects;

namespace Domain.Partners.Interfaces;
public interface ICompanyRepository
{
    Task<Guid> AddAsync(Company company, CancellationToken cancellationToken = default);
    Task<List<Company>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Company?> GetByIdReadOnlyAsync(CompanyId companyId, CancellationToken cancellationToken);
    Task<Company?> GetByIdWithUsersAsync(CompanyId companyId, CancellationToken cancellationToken);
    Task<Company?> GetByCuitAsync(Cuit cuit, CancellationToken cancellationToken = default);
    Task<Company?> GetByIdAsync(CompanyId id, CancellationToken cancellationToken = default);
}
