using Application.Documents.Common.DTOs;
using Application.Documents.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Common;

namespace Web.API.Controllers.Documents;

[Route("documents")]
public class DocumentsController : ApiController
{
    private readonly IDocumentService _service;

    public DocumentsController(IDocumentService service)
    {
        _service = service ?? throw new ArgumentException(nameof(service));
    }

    [HttpGet("all")]
    [Authorize(Policy = "UserOnly")]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? filter = null)
    {
        DocumentAssignedDTO dto = new DocumentAssignedDTO { Filter = filter, Page = page, PageSize = pageSize };
        var result = await _service.GetDocumentsAsignedToPaginatedAsync(dto);

        return result.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id:guid}/download")]
    [Authorize(Policy = "UserOnly")]
    public async Task<IActionResult> Download(Guid id)
    {
        var result = await _service.GetDocumentByIdAsync(id);

        return result.Match<IActionResult>(
            success => File(success.Content, success.ContentType, success.FileName),
            error => NotFound(error)
        );
    }
}
