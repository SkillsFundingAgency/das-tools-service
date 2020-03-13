using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolService.Core.Entities;
using SFA.DAS.ToolService.Core.IServices;
using SFA.DAS.ToolService.Web.Configuration;
using SFA.DAS.ToolService.Web.Extensions;
using SFA.DAS.ToolService.Web.Models.Admin;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Web.Controllers.Admin
{
    [Authorize(Policy = "admin")]
    [Route("admin/manage-roles/assignments")]
    public class AdminRoleAssignmentController : BaseController<AdminRoleAssignmentController>
    {
        private readonly IApplicationService applicationService;
        private readonly IRoleService roleService;

        public AdminRoleAssignmentController(IApplicationService _applicationService, IRoleService _roleService)
        {
            applicationService = _applicationService;
            roleService = _roleService;
        }

        [HttpGet("{roleId}", Name = AdminRoleAssignmentRouteNames.ManageRoleAssignment)]
        public async Task<IActionResult> ManageRoleAssignment(int roleId)
        {
            var assignedApplications = await applicationService.GetApplicationsForRoleId(roleId);
            var unassignedApplications = await applicationService.GetUnassignedApplicationsForRole(roleId);

            var role = await roleService.GetRole(roleId);
            return View(new ApplicationsForRoleViewModel
            {
                RoleId = roleId,
                RoleName = role.Name,
                AssignedApplications = assignedApplications,
                UnassignedApplications = unassignedApplications
            });
        }

        // update mappings
        [HttpPost("{roleId}/add", Name = AdminRoleAssignmentRouteNames.AddApplication)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRoleToApplication(ApplicationsForRoleViewModel model)
        {
            await applicationService.AssignApplicationToRole(model.ApplicationId, model.RoleId);
            TempData.Put("model", new { Message = "The requested role assignment has been updated." });
            return RedirectToAction(nameof(AdminController.AdminActionComplete), typeof(AdminController));
        }

        [HttpPost("{roleId}/remove", Name = AdminRoleAssignmentRouteNames.RemoveApplication)]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveApplicationFromRole(ApplicationsForRoleViewModel model)
        {
            applicationService.RemoveApplicationFromRole(model.ApplicationId, model.RoleId);
            TempData.Put("model", new { Message = "The requested role assignment has been updated." });
            return RedirectToAction(nameof(AdminController.AdminActionComplete), typeof(AdminController));
        }
    }
}
