using Application.Common.Dtos;
using Application.Partners.Dtos;
using Application.Partners.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Common;

namespace Web.API.Controllers.Partners;

[Route("partners/company")]
public class CompanyController(ICompanyService companyService) : ApiController
{
    private readonly ICompanyService _companyService = companyService;

    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetPaged([FromQuery] PaginateDTO filter)
    {
        var companyFilter = new CompanyFilterDto { Page = filter.Page, PageSize = filter.PageSize, Filter = filter.Filter };
        var result = await _companyService.GetPagedAsync(companyFilter);

        return result.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _companyService.GetByIdAsync(id);

        return result.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] CreateCompanyRequest request)
    {
        var result = await _companyService.CreateAsync(request.Name, request.Cuit);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    [HttpPut("edit/{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCompanyRequest request)
    {
        if (id != request.Id)
            return BadRequest("Route ID and body ID do not match.");

        var result = await _companyService.UpdateAsync(request.Id, request.Name, request.Cuit);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _companyService.DeleteAsync(id);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }
}

public record CreateCompanyRequest(string Name, string Cuit);
public record UpdateCompanyRequest(Guid Id, string Name, string Cuit);
