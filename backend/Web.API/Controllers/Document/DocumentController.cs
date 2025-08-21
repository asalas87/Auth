using Application.Documents.Common.DTOs;
using Application.Documents.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Common;

namespace Web.API.Controllers.Documents;

[Authorize(Policy = "UserOnly")]
[Route("document/management")]
public class DocumentsController : ApiController
{
    private readonly IDocumentService _service;

    public DocumentsController(IDocumentService service)
    {
        _service = service ?? throw new ArgumentException(nameof(service));
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? filter = null)
    {
        DocumentAssignedDTO dto = new DocumentAssignedDTO { Filter = filter, Page = page, PageSize = pageSize };
        var result = await _service.GetDocumentsAsignedToPaginatedAsync(dto);

        return result.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }
}
