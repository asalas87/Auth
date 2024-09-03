using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers.Common
{
    public class ErrorController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/error")]
        public IActionResult Error()
        {
            Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            return Problem();
        }
    }
}
