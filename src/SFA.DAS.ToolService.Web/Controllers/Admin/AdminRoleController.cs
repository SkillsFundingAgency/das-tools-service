using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Core.IServices;

namespace SFA.DAS.ToolService.Web.Controllers.Admin
{
    public class AdminRoleController : BaseController
    {
        private readonly ILogger logger;
        private readonly IApplicationService applicationService;

        public AdminRoleController(ILogger<AdminRoleController> _logger,
            IApplicationService _applicationService)
        {
            logger = _logger;
            applicationService = _applicationService;
        }

        [HttpGet("admin/manage-roles")]
        public IActionResult ManageRoles()
        {
            return View();
        }
    }
}
