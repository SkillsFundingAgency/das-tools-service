using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Core.IServices;
using SFA.DAS.ToolService.Web.Models;
using SFA.DAS.ToolService.Web.Models.Admin;

namespace SFA.DAS.ToolService.Web.Controllers.Admin
{
    public class AdminRoleAssignmentController : BaseController
    {
        private readonly ILogger logger;
        private readonly IApplicationService applicationService;

        public AdminRoleAssignmentController(ILogger<AdminRoleAssignmentController> _logger,
            IApplicationService _applicationService)
        {
            logger = _logger;
            applicationService = _applicationService;
        }


        // list roles
        [HttpGet("admin/role-assignments", Name="RoleAssignment")]
        public async Task<IActionResult> GetAvailableRoles()
        {
            var roles = await applicationService.GetRoles();
            var model = new GetAvailableRolesViewModel { Roles = roles };
            return View(model);
        }

        [HttpPost("admin/role-assignments")]
        public IActionResult RoleAssignmentHandleChoice(GetAvailableRolesViewModel model)
        {
            return RedirectToAction(model.Action, new { roleId = model.RoleId });
        }

        // list unassigned applications for role id
        [HttpGet("admin/role-assignments/{roleId}/add")]
        public async Task<IActionResult> GetUnassignedApplicationsForRole(int roleId)
        {
            if (roleId == 0)
            {
                return new BadRequestResult();
            }

            var applications = await applicationService.GetUnassignedApplicationsForRole(roleId);

            return View(new ApplicationsForRoleViewModel
            {
                RoleId = roleId,
                Applications = applications
            });
        }


        // list all applications for role id
        [HttpGet("admin/role-assignments/{roleId}/remove")]
        public async Task<IActionResult> GetApplicationsForRole(int roleId)
        {

            var applications = await applicationService.GetApplicationsForRoleId(roleId);
            return View(new ApplicationsForRoleViewModel
            {
                RoleId = roleId,
                Applications = applications
            });
        }

        // update mappings
        [HttpPost("admin/role-assignments/{roleId}/add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRoleToApplication(ApplicationsForRoleViewModel model)
        {

            await applicationService.AssignApplicationToRole(model.ApplicationId, model.RoleId);
            return RedirectToAction(nameof(AdminController.ActionComplete), typeof(AdminController), new { message = "The requested role assignment has been updated." });
        }

        [HttpPost("admin/role-assignments/{roleId}/remove")]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveApplicationFromRole(ApplicationsForRoleViewModel model)
        {

            applicationService.RemoveApplicationFromRole(model.ApplicationId, model.RoleId);
            return RedirectToAction(nameof(AdminController.ActionComplete), typeof(AdminController), new { message = "The requested role assignment has been updated." });
        }
    }
}
