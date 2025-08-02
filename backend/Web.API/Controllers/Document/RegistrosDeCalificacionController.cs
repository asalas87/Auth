using Application.Common.Dtos;
using Application.Documents.Services;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Common;

namespace Web.API.Controllers.Document;

[Route("api/[controller]")]
[ApiController]
public class RegistrosDeCalificacionController : ApiController
{
    private readonly IDocumentService _service;

    public RegistrosDeCalificacionController(IDocumentService service)
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

    // GET api/<RegistrosDeCalificacionController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<RegistrosDeCalificacionController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<RegistrosDeCalificacionController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
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
