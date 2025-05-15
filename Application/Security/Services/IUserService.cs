using Application.Common.Dtos;
using Application.Common.Responses;
using Application.Security.Common.DTOS;
using Application.Security.Common.Responses;
using ErrorOr;

namespace Application.Security.Services;
public interface IUserService
{
    public Task<ErrorOr<LoginResponse>> RegisterUserAsync(RegisterDTO userDto);
    public Task<ErrorOr<LoginResponse>> LoginUserAsync(LoginDTO userDto);
    public Task<ErrorOr<PaginatedResult<UserDTO>>> GetUsersPaginatedAsync(PaginateDto loginDTO);
}
