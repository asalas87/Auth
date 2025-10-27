using Application.Common.Dtos;
using Application.Common.Responses;
using Application.Security.Common.DTOs;
using Application.Security.Common.DTOS;
using ErrorOr;

namespace Application.Security.Services;
public interface IUserService
{
    public Task<ErrorOr<PaginatedResult<UserDTO>>> GetUsersPaginatedAsync(PaginateDTO paginateDTO);
    public Task<ErrorOr<List<UserDTO>>> GetUsersAsync();
    Task<ErrorOr<SuccessResponse>> DeleteUserAsync(Guid userId);
    Task<ErrorOr<SuccessResponse>> EditUserAsync(EditUserRequest dto);
    Task<ErrorOr<SuccessResponse>> CreateUserAsync(EditUserRequest dto);
    Task<ErrorOr<UserEditDTO>> GetUserByIdAsync(Guid userId);
}
