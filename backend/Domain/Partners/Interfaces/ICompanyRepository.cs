using Domain.Partners.Entities;

namespace Domain.Partners.Interfaces;
public interface ICompanyRepository
{
    Task<List<Company>> GetAllAsync(CancellationToken cancellationToken = default);
}
