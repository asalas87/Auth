using Application.Common.Dtos;
using Application.Documents.ProcedureSpecification.Dtos;
using Application.Documents.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Common;

namespace Web.API.Controllers.Document;

[Route("document/[controller]")]
[ApiController]
public class ProcedureSpecificationController(IDocumentService service) : ApiController
{
    private readonly IDocumentService _service = service ?? throw new ArgumentException(null, nameof(service));

    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? filter = null)
    {
        var result = await _service.GetProcedureSpecificationsPaginatedAsync(new PaginateDTO
        {
            Page = page,
            PageSize = pageSize,
            Filter = filter
        });

        return result.Match(value => Ok(value), errors => Problem(errors));
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetProcedureSpecificationByIdAsync(id);
        return result.Match(value => Ok(value), errors => Problem(errors));
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Post([FromForm] ProcedureSpecificationDTO dto)
    {
        var result = await _service.CreateProcedureSpecificationAsync(dto);
        return result.Match(value => Ok(value), errors => Problem(errors));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Put(Guid id, [FromForm] ProcedureSpecificationEditDTO dto)
    {
        dto.Id = id;
        var result = await _service.UpdateProcedureSpecificationAsync(dto);
        return result.Match(value => Ok(value), errors => Problem(errors));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteProcedureSpecificationAsync(id);
        return result.Match(value => Ok(value), errors => Problem(errors));
    }
}
