using Application.Controls.Dtos;
using ErrorOr;

namespace Application.Controls.Services;
public interface IControlService
{
    Task<ErrorOr<List<CompanyLookupDto>>> GetAllCompaniesAsync(CancellationToken cancellationToken = default);
    //Task<List<UserLookupDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);
}
