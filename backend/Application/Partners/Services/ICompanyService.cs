using Application.Common.Dtos;
using Application.Common.Responses;
using Application.Partners.Dtos;
using ErrorOr;

namespace Application.Partners.Services;

public interface ICompanyService
{
    Task<ErrorOr<PaginatedResult<CompanyDto>>> GetPagedAsync(CompanyFilterDto filter);
    Task<ErrorOr<CompanyDto>> GetByIdAsync(Guid id);
    Task<ErrorOr<SuccessResponse>> CreateAsync(string name, string cuit);
    Task<ErrorOr<SuccessResponse>> UpdateAsync(Guid id, string name, string cuit);
    Task<ErrorOr<SuccessResponse>> DeleteAsync(Guid id);
}
