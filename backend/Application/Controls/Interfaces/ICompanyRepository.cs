using Domain.Partners.Entities;
using Domain.ValueObjects;

namespace Application.Controls.Interfaces;
public interface ICompanyRepository
{
    Task<List<Company>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Company?> GetByCuitAsync(Cuit cuit, CancellationToken cancellationToken = default);
    Task<Company?> GetByIdAsync(CompanyId id, CancellationToken cancellationToken = default);
    Task<Company?> GetByIdReadOnlyAsync(CompanyId id, CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(Company company, CancellationToken cancellationToken = default);
    Task<Company?> GetByIdWithUsersAsync(CompanyId id, CancellationToken cancellationToken = default);
}
