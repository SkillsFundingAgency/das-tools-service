using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace SFA.DAS.ToolService.Web.Controllers
{
    [Route("error")]
    public class ErrorController : BaseController
    {

        [Route("403")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Route("404")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [Route("500")]
        public IActionResult ApplicationError()
        {
            return View();
        }
    }
}
