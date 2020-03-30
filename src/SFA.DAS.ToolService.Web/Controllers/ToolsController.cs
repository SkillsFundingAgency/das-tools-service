using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolService.Core.IServices;
using SFA.DAS.ToolService.Web.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Web.Controllers
{
    [Route("home")]
    public class ToolsController : BaseController<ToolsController>
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IApplicationService applicationService;

        public ToolsController(IHttpContextAccessor _contextAccessor,
            IApplicationService _applicationService)
        {
            contextAccessor = _contextAccessor;
            applicationService = _applicationService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var user = contextAccessor.HttpContext.User;
            var userName = user.FindFirst(ClaimTypes.NameIdentifier).Value;
            var roles = user.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray();
            var applications = await applicationService.GetApplicationsForRole(roles);
            var model = new ToolsHomeViewModel
            {
                UserName = userName,
                Applications = applications
            };

            return View(model);
        }
    }
}
