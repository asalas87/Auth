using Application.Controls.Dtos;
using Application.Controls.Services;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("company/list")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetCompanies()
    {
        var result = await _service.GetAllCompaniesAsync();

        return result.Match(
            value => Ok(result.Value),
            errors => Problem(result.Errors)
            );
    }

    [HttpGet("company/by-cuit")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetCompanyByCuit([FromQuery] string cuit)
    {
        var result = await _service.GetCompanyByCuitAsync(cuit);
        return result.IsError ? BadRequest(result.FirstError) : Ok(result.Value);
    }

    [HttpPost("company")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto dto)
    {
        var result = await _service.CreateCompanyAsync(dto);
        return result.IsError ? BadRequest(result.FirstError) : Ok(result.Value);
    }

    [HttpGet("role/list")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetRolesAsync();

        return result.Match(
            value => Ok(result.Value),
            errors => Problem(result.Errors)
            );
    }
}
