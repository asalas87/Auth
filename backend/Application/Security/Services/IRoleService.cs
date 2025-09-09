using Application.Security.Common.DTOS;
using ErrorOr;

namespace Application.Security.Services;
public interface IRoleService
{
    Task<ErrorOr<List<RoleDTO>>> GetRolesAsync();
}
