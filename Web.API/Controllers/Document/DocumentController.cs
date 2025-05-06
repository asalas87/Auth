using Application.Documents.Management.DTOs;
using Application.Documents.Management.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Common;
using Web.API.Common.Extensions;
using Web.API.Controllers.Common;

namespace Web.API.Controllers.Documents;

[Authorize]
[Route("documents")]
public class DocumentsController : ApiController
{
    private readonly IDocumentService _service;

    public DocumentsController(IDocumentService service)
    {
        _service = service ?? throw new ArgumentException(nameof(service));
    }

    [HttpPost("upload", Name = "upload")]
    public async Task<IActionResult> Upload([FromForm] DocumentUploadDTO dto)
    {
        var userId = HttpContext.GetUserIdOrThrow();
        dto.UploadedBy = userId;

        var uploadResult = await _service.UploadDocumentAsync(dto);

        return uploadResult.Match(
            _ => Ok(),
            errors => Problem(errors)
        );
    }
}
