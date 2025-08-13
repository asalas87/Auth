using Application.Security.Common.DTOS;
using Application.Security.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Common;

namespace Web.API.Controllers.Security;
[Route("security/[controller]")]
public class AccountController : ApiController
{
    private readonly IUserService _service;
    public AccountController(IUserService service)
    {
        _service = service ?? throw new ArgumentException(nameof(service));
    }

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
}
