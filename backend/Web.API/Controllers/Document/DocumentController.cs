using Application.Documents.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Common.Request;
using Web.API.Controllers.Common;

namespace Web.API.Controllers.Document;

[Route("documents")]
public class DocumentsController(IDocumentService service) : ApiController
{
    private readonly IDocumentService _service = service ?? throw new ArgumentException(null, nameof(service));

    [HttpGet("all")]
    [Authorize(Policy = "UserOnly")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetUserDocumentsAsync();

        return result.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id:guid}/download")]
    [Authorize(Policy = "UserOnly")]
    public async Task<IActionResult> Download(Guid id)
    {
        var result = await _service.DownloadAsync(id);

        return result.Match<IActionResult>(
            success => File(success.Content, success.ContentType, success.FileName),
            error => NotFound(error)
        );
    }

    [HttpPost("download-multiple")]
    [Authorize(Policy = "UserOnly")]
    public async Task<IActionResult> DownloadMultiple([FromBody] MultipleIdsRequest request)
    {
        var result = await _service.DownloadMultipleAsync(request.Ids);

        return result.Match<IActionResult>(
            success => File(success.Content, success.ContentType, success.FileName),
            error => NotFound(error)
        );
    }

    //[HttpDelete("{id}")]
    //[Authorize(Policy = "UserOnly")]
    //public async Task<IActionResult> Delete(Guid id)
    //{
    //    var result = await _service.DeleteCertificateAsync(id);
    //    return result.Match(
    //        success => Ok(success),
    //        errors => Problem(errors)
    //    );
    //}
}
