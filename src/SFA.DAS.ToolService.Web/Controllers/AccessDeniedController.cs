using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.ToolService.Authentication.Entities;

namespace SFA.DAS.ToolService.Web.Controllers
{
    public class AccessDeniedController : Controller
    {
        private readonly ILogger logger;
        public AccessDeniedController(ILogger<AccessDeniedController> _logger)
        {
            logger = _logger;
        }

        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
