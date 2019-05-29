using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Web.Models;

namespace SFA.DAS.ToolService.Web.Controllers
{
    [Authorize(Policy="ValidOrgsOnly")]
    public class ToolsController : Controller
    {
        private readonly ILogger logger;
        
        public ToolsController(ILogger<AccountController> _logger)
        {
            logger = _logger;
        }

        [Route("Home")]
        public IActionResult Home()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
