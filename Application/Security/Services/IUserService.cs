using Application.Security.Common.DTOS;
using Application.Security.Common.Responses;
using ErrorOr;

namespace Application.Security.Services;
public interface IUserService
{
    public Task<ErrorOr<Guid>> RegisterUserAsync(RegisterDTO userDto);
    public Task<ErrorOr<LoginResponse>> LoginUserAsync(LoginDTO userDto);
}
