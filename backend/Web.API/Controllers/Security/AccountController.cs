using Application.Security.Common.DTOS;
using Application.Security.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Common;

namespace Web.API.Controllers.Security;
[Route("security/[controller]")]
public class AccountController(IAuthenticationService service) : ApiController
{
    private readonly IAuthenticationService _service = service ?? throw new ArgumentException(null, nameof(service));

    [AllowAnonymous]
    [HttpPost("register", Name = "register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        var createRegisterResult = await _service.RegisterUserAsync(dto);

        return createRegisterResult.Match(
            value => Ok(createRegisterResult.Value),
            errors => Problem(createRegisterResult.Errors)
            );
    }

    [AllowAnonymous]
    [HttpPost("login", Name = "login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var createLoginResult = await _service.LoginUserAsync(dto);

        return createLoginResult.Match(
            value => Ok(createLoginResult.Value),
            errors => Problem(createLoginResult.Errors)
        );
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO dto)
    {
        var result = await _service.RefreshTokenAsync(dto.RefreshToken);

        return result.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    [AllowAnonymous]
    [HttpPost("activate")]
    public async Task<IActionResult> Activate([FromBody] ActivateAccountDTO dto)
    {
        var result = await _service.ActivateUserAsync(dto);

        return result.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    [AllowAnonymous]
    [HttpPost("forgot-password", Name = "forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto, CancellationToken cancellationToken)
    {
        var result = await _service.RequestPasswordResetAsync(dto, cancellationToken);

        return result.Match(
            value => Ok(new { message = "Si el email está registrado, recibirás un enlace para reiniciar tu contraseña." }),
            errors => Problem(errors)
        );
    }

    [AllowAnonymous]
    [HttpPost("reset-password", Name = "reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto, CancellationToken cancellationToken)
    {
        var result = await _service.ResetPasswordAsync(dto, cancellationToken);

        return result.Match(
            value => NoContent(),
            errors => Problem(errors)
        );
    }
}
