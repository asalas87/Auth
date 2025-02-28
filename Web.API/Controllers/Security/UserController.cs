using Application.Security.Common.DTOS;
using Application.Security.Services;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Common;

namespace Web.API.Controllers.Security
{
    [Route("security/users")]
    public class UserController : ApiController
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service ?? throw new ArgumentException(nameof(service));
        }

        [HttpPost("register", Name = "register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var createRegisterResult = await _service.RegisterUserAsync(dto);

            return createRegisterResult.Match(
                value => Ok(createRegisterResult.Value),
                errors => Problem(createRegisterResult.FirstError.Description)
                );
        }

        [HttpPost("login", Name = "login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var createLoginResult = await _service.LoginUserAsync(dto);

            return createLoginResult.Match(
                value => Ok(createLoginResult.Value),
                errors => Problem(createLoginResult.FirstError.Description)
            );
        }
    }
}
