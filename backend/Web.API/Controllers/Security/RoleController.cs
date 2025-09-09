using Application.Security.Services;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Common;

namespace Web.API.Controllers.Security;

[Route("security/[controller]")]
public class RoleController : ApiController
{
    private readonly IRoleService _service;
    public RoleController(IRoleService service)
    {
        _service = service ?? throw new ArgumentException(nameof(service));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetRolesAsync();

        return result.Match(
            value => Ok(result.Value),
            errors => Problem(result.Errors)
            );
    }
}
