using Domain.Partners.Entities;

namespace Application.Controls.Interfaces;
public interface IControlCompanyRepository
{
    Task<List<Company>> GetAllAsync(CancellationToken cancellationToken = default);
}
