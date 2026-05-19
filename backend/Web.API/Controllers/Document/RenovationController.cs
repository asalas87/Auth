using Application.Common.Dtos;
using Application.Documents.Renovation.Dtos;
using Application.Documents.Renovation.DTOs;
using Application.Documents.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Common;

namespace Web.API.Controllers.Document;

[Route("document/[controller]")]
[ApiController]
public class RenovationController(IDocumentService service) : ApiController
{
    private readonly IDocumentService _service = service ?? throw new ArgumentException(null, nameof(service));

    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? filter = null)
    {
        PaginateDTO dto = new (){ Filter = filter, Page = page, PageSize = pageSize };
        var result = await _service.GetRenovationsPaginatedAsync(dto);

        return result.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetRenovationByIdAsync(id);

        return result.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Post([FromForm] RenovationDTO dto)
    {
        var result = await _service.CreateRenovationAsync(dto);
        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Put(Guid id, [FromBody] RenovationEditDTO dto)
    {
        var result = await _service.UpdateRenovationAsync(dto);
        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteRenovationAsync(id);
        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }
}
