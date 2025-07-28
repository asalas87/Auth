using Application.Common.Dtos;
using Application.Partners.Services;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Common;

namespace Web.API.Controllers.Partners
{
    public class CompanyController : ApiController
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        // GET /partners/company/list
        //[HttpGet("list")]
        //public async Task<IActionResult> GetAllForCombo()
        //{
        //    var result = await _companyService.GetAllForComboAsync();
        //    return Ok(result);
        //}

        //// GET /partners/company?page=1&pageSize=10&name=foo
        //[HttpGet]
        //public async Task<IActionResult> GetPaged([FromQuery] PaginateDTO filter)
        //{
        //    var result = await _companyService.GetPagedAsync(filter);
        //    return Ok(result);
        //}
    }
}
