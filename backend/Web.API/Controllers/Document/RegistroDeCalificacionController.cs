using Application.Common.Dtos;
using Application.Documents.Certificate.DTOs;
using Application.Documents.Services;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Common;

namespace Web.API.Controllers.Document;

[Route("document/[controller]")]
[ApiController]
public class RegistroDeCalificacionController : ApiController
{
    private readonly IDocumentService _service;

    public RegistroDeCalificacionController(IDocumentService service)
    {
        _service = service ?? throw new ArgumentException(nameof(service));
    }
    // GET: api/<RegistrosDeCalificacionController>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? filter = null)
    {
        PaginateDTO dto = new PaginateDTO { Filter = filter, Page = page, PageSize = pageSize };
        var result = await _service.GetCertificatesPaginatedAsync(dto);

        return result.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    // POST api/<RegistrosDeCalificacionController>
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Post([FromForm] CertificateDTO dto)
    {
        var result = await _service.CreateCertificateAsync(dto);
        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    // PUT api/<RegistrosDeCalificacionController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] CertificateEditDTO dto)
    {
        var result = await _service.UpdateCertificateAsync(dto);
        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    // DELETE api/<RegistrosDeCalificacionController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteCertificateAsync(id);
        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }
}
