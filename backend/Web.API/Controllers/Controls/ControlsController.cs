using Application.Controls.Services;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Common;

namespace Web.API.Controllers.Controls;

[Route("controls")]
public class ControlsController : ApiController
{
    private readonly IControlService _service;
    public ControlsController(IControlService service)
    {
        _service = service ?? throw new ArgumentException(nameof(service));
    }

    [HttpGet("companies/list")]
    public async Task<IActionResult> GetCompanies()
    {
        var result = await _service.GetAllCompaniesAsync();

        return result.Match(
            value => Ok(result.Value),
            errors => Problem(result.Errors)
            );
    }
}
