using Domain.Partners.Entities;
using Domain.ValueObjects;

namespace Domain.Partners.Interfaces;
public interface ICompanyRepository
{
    Task AddAsync(Company company, CancellationToken cancellationToken = default);
    Task<List<Company>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Company?> GetByIdReadOnlyAsync(CompanyId companyId, CancellationToken cancellationToken);
    Task<Company?> GetByIdWithUsersAsync(CompanyId companyId, CancellationToken cancellationToken);
    Task<Company?> GetByCuitAsync(Cuit cuit, CancellationToken cancellationToken = default);
    Task<Company?> GetByIdAsync(CompanyId id, CancellationToken cancellationToken = default);
    Task<Company?> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default);
    Task<(List<Company> Companies, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? filter, CancellationToken cancellationToken = default);
    void Update(Company company);
    void Delete(Company company);
}
