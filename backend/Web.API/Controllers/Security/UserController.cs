using Application.Common.Dtos;
using Application.Security.Common.DTOs;
using Application.Security.Common.DTOS;
using Application.Security.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Common;

namespace Web.API.Controllers.Security;

[Route("security/[controller]")]
public class UserController : ApiController
{
    private readonly IUserService _service;
    public UserController(IUserService service)
    {
        _service = service ?? throw new ArgumentException(nameof(service));
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? filter = null)
    {
        PaginateDTO dto = new PaginateDTO { Filter = filter, Page = page, PageSize = pageSize };
        var result = await _service.GetUsersPaginatedAsync(dto);

        return result.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] EditUserRequest dto)
    {
        if (id != dto.Id)
            return BadRequest("Route ID and body ID do not match.");

        var result = await _service.EditUserAsync(dto);

        return result.Match(
            success => Ok(success),
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
}
