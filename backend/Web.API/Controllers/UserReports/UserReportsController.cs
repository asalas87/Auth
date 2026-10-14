using Application.UserReports;
using Application.UserReports.SendUserReport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Common;
using Web.API.Security;

namespace Web.API.Controllers.UserReports;

[ApiController]
[AllowAnonymous]
[Route("api/user-reports")]
public class UserReportsController(
    IUserReportService service) : ApiController
{
    private readonly IUserReportService _service = service
        ?? throw new ArgumentNullException(nameof(service));

    [HttpPost]
    [ProtectedAction("user-report")]
    public async Task<IActionResult> Send(
        [FromBody] SendUserReportCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _service.SendAsync(command, cancellationToken);

        return result.Match(
            _ => Accepted(new { message = "Report received." }),
            errors => Problem(errors));
    }
}