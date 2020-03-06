using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolService.Core.IServices;
using SFA.DAS.ToolService.Web.Configuration;
using SFA.DAS.ToolService.Web.Extensions;
using SFA.DAS.ToolService.Web.Models.Admin;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Web.Controllers.Admin
{
    [Authorize(Policy = "admin")]
    [Route("admin/role-assignments")]
    public class AdminRoleAssignmentController : BaseController<AdminRoleAssignmentController>
    {
        private readonly IApplicationService applicationService;
        private readonly IRoleService roleService;

        public AdminRoleAssignmentController(IApplicationService _applicationService, IRoleService _roleService)
        {
            applicationService = _applicationService;
            roleService = _roleService;
        }

        // list roles
        [HttpGet("", Name = AdminRoleAssignmentRouteNames.RoleAssignment)]
        public async Task<IActionResult> Index()
        {
            var roles = await roleService.GetRoles();
            return View(new RoleAssignmentsViewModel { Roles = roles });
        }

        [HttpPost]
        public IActionResult IndexHandleChoice(RoleAssignmentsViewModel model)
        {
            return RedirectToRoute("AssignmentChoice", new { roleId = model.RoleId });
        }

        [HttpGet("{roleId}", Name = AdminRoleAssignmentRouteNames.AssignmentChoice)]
        public IActionResult AssignmentChoice(int roleId)
        {
            return View(new AssignmentChoiceViewModel());
        }

        [HttpPost("{roleId}", Name = AdminRoleAssignmentRouteNames.HandleAssignmentChoice)]
        public IActionResult HandleAssignmentChoice(int roleId, AssignmentChoiceViewModel model)
        {
            return RedirectToAction(model.Action.ToString(), new { roleId = roleId });
        }

        // list unassigned applications for role id
        [HttpGet("{roleId}/add", Name = AdminRoleAssignmentRouteNames.GetUnassignedApplications)]
        public async Task<IActionResult> GetUnassignedApplications(int roleId)
        {
            if (roleId == 0)
            {
                return new BadRequestResult();
            }

            var applications = await applicationService.GetUnassignedApplicationsForRole(roleId);
            var role = await roleService.GetRole(roleId);

            return View(new ApplicationsForRoleViewModel
            {
                RoleId = roleId,
                RoleName = role.Name,
                Applications = applications
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

        // list all applications for role id
        [HttpGet("{roleId}/remove", Name = AdminRoleAssignmentRouteNames.GetAssignedApplications)]
        public async Task<IActionResult> GetAssignedApplications(int roleId)
        {
            var applications = await applicationService.GetApplicationsForRoleId(roleId);
            var role = await roleService.GetRole(roleId);
            return View(new ApplicationsForRoleViewModel
            {
                RoleId = roleId,
                RoleName = role.Name,
                Applications = applications
            });
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
