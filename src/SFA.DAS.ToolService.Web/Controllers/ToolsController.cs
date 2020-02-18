using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Web.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ToolService.Core.IRepositories;
using SFA.DAS.ToolService.Core.IServices;
using SFA.DAS.ToolService.Core.Entities;

namespace SFA.DAS.ToolService.Web.Controllers
{
    // [Authorize(Policy="ValidOrgsOnly")]
    [Authorize]
    public class ToolsController : BaseController
    {
        private readonly ILogger logger;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IApplicationService applicationService;

        public ToolsController(ILogger<ToolsController> _logger,
            IHttpContextAccessor _contextAccessor,
            IApplicationService _applicationService)
        {
            logger = _logger;
            contextAccessor = _contextAccessor;
            applicationService = _applicationService;
        }

        [Route("home")]
        public async Task<IActionResult> Home()
        {
            var user = contextAccessor.HttpContext.User;
            var userName = user.FindFirst(ClaimTypes.NameIdentifier).Value;
            var roles = user.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray();
            var applications = await applicationService.GetApplicationsForRole(roles);
            var model = new ToolsHomeViewModel {
                UserName = userName,
                Applications = applications
            };

            return View(model);
        }
    }
}
