using Application.Common.Dtos;
using Application.Documents.Common.DTOs;
using Application.Documents.Management.DTOs;
using Application.Documents.Services;
using Application.Security.Common.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Common;

namespace Web.API.Controllers.Documents;
[Route("documents/management")]
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
        PaginateDTO dto = new PaginateDTO { Filter = filter, Page = page, PageSize = pageSize };
        var result = await _service.GetDocumentsPaginatedAsync(dto);

        return result.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    [Authorize]
    [HttpPost("upload", Name = "upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadFile([FromForm] CreateDocumentDTO dto)
     {
        var result = await _service.CreateDocumentAsync(dto);

        return result.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }
    //[HttpGet(Name = "get-documents-assigned")]
    //public async Task<IActionResult> GetAllAsigned([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string assignedTo = "", [FromQuery] string? filter = null)
    //{
    //    DocumentAssignedDTO dto = new DocumentAssignedDTO { Filter = filter, Page = page, PageSize = pageSize, AssignedTo = Guid.Parse(assignedTo) };
    //    var result = await _service.GetDocumentsAsignedToPaginatedAsync(dto);

    //    return result.Match(
    //        value => Ok(value),
    //        errors => Problem(errors)
    //    );
    //}
}
