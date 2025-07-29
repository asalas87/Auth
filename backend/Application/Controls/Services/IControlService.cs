using Application.Controls.Dtos;
using ErrorOr;

namespace Application.Controls.Services;
public interface IControlService
{
    Task<ErrorOr<List<CompanyLookupDto>>> GetAllCompaniesAsync(CancellationToken cancellationToken = default);
    Task<ErrorOr<CompanyLookupDto>> GetCompanyByCuitAsync(string cuit, CancellationToken cancellationToken = default);
    Task<ErrorOr<Guid>> CreateCompanyAsync(CreateCompanyDto dto, CancellationToken cancellationToken = default);
}
