using Application.Documents.Analysis.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers.Document
{
    [ApiController]
    [Route("documents")]
    public class DocumentAnalysisController(IDocumentAnalysisService service) : ControllerBase
    {
        private readonly IDocumentAnalysisService _service = service;

        [HttpPost("analyze")]
        [Consumes("multipart/form-data")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Analyze(
            IFormFile file,
            [FromForm] DocumentType documentType)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Archivo inválido");

            var result = await _service.AnalyzeAsync(file, documentType);

            return Ok(result);
        }
    }
}
