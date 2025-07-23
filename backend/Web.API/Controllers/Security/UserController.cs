using Application.Common.Dtos;
using Application.Security.Common.DTOs;
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
                errors => Problem(createRegisterResult.Errors)
                );
        }

        [HttpPost("login", Name = "login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var createLoginResult = await _service.LoginUserAsync(dto);

            return createLoginResult.Match(
                value => Ok(createLoginResult.Value),
                errors => Problem(createLoginResult.Errors)
            );
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO dto)
        {
            var result = await _service.RefreshTokenAsync(dto.RefreshToken);

            return result.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }

        [HttpGet(Name = "get-users")]
        public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? filter = null)
        {
            PaginateDTO dto = new PaginateDTO { Filter = filter, Page = page, PageSize = pageSize};  
            var result = await _service.GetUsersPaginatedAsync(dto);

            return result.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }

        [HttpGet("getAll", Name = "get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _service.GetUsersAsync();

            return result.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteUserAsync(id);
            return result.Match(
                success => Ok(success),
                errors => Problem(errors)
            );
        }

        [HttpPut("{id:guid}", Name = "EditUser")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] EditUserRequest dto)
        {
            if (id != dto.Id)
                return BadRequest("Route ID and body ID do not match.");

            var result = await _service.EditUserAsync(dto);

            return result.Match(
                success => Ok(success),
                errors => Problem(errors)
            );
        }
    }
}
