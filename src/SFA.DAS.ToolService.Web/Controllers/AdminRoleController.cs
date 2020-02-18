using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Core.IServices;
using SFA.DAS.ToolService.Web.Models.Admin;

namespace SFA.DAS.ToolService.Web.Controllers
{
    public class AdminRoleController : Controller
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
