using Application.Security.Common.DTOS;
using Application.Security.Common.Responses;
using Domain.Security.Entities;
using ErrorOr;

namespace Application.Security.Services;

public interface IAuthenticationService
{
    public Task<ErrorOr<LoginResponse>> RegisterUserAsync(RegisterDTO registerDTO);
    public Task<ErrorOr<LoginResponse>> LoginUserAsync(LoginDTO loginDTO);
    public Task<ErrorOr<LoginResponse>> RefreshTokenAsync(string refreshToken);
    public Task<ErrorOr<LoginResponse>> ActivateUserAsync(ActivateAccountDTO dto);
    public Task<ErrorOr<bool>> RequestPasswordResetAsync(ForgotPasswordDTO dto, CancellationToken cancellationToken = default);
    public Task<ErrorOr<bool>> ResetPasswordAsync(ResetPasswordDTO dto, CancellationToken cancellationToken = default);
    Task<string> GenerateRefreshTokenAsync(Guid userId);
    string GenerateAccessToken(User user);
}
