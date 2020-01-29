using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Web.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace SFA.DAS.ToolService.Web.Controllers
{
    // [Authorize(Policy="ValidOrgsOnly")]
    [Authorize]
    public class ToolsController : Controller
    {
        private readonly ILogger logger;
        private readonly IHttpContextAccessor contextAccessor;

        public ToolsController(ILogger<AccountController> _logger, IHttpContextAccessor _contextAccessor)
        {
            logger = _logger;
            contextAccessor = _contextAccessor;
        }

        [Route("Home")]
        public IActionResult Home()
        {
            var user = contextAccessor.HttpContext.User;

            var currentUser = user.FindFirst(ClaimTypes.NameIdentifier);
            var model = new ToolsHomeViewModel{
                UserName = currentUser.Value,
                HasApprenticeshipsRole = user.IsInRole("Apprenticeship Service")
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
