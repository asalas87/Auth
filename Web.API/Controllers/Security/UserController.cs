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

        [HttpPost(Name = "register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var createRegisterResult = await _service.RegisterUserAsync(dto);

            return createRegisterResult.Match(
                value => Ok(),
                errors => Problem(errors)
                );
        }

        [HttpGet(Name = "login")]
        public async Task<IActionResult> Login([FromQuery] LoginDTO dto)
        {
            var createLoginResult = await _service.LoginUserAsync(dto);

            return createLoginResult.Match(
                value => Ok(),
                errors => Problem(errors)
                );
        }
    }
}
