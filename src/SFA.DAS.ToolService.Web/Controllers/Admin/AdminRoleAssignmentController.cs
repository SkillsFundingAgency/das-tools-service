using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolService.Web.Configuration;
using SFA.DAS.ToolService.Core.IServices;
using SFA.DAS.ToolService.Web.Extensions;
using SFA.DAS.ToolService.Web.Models.Admin;

namespace SFA.DAS.ToolService.Web.Controllers.Admin
{
    [Authorize(Policy = "admin")]
    [Route("admin/role-assignments")]
    public class AdminRoleAssignmentController : BaseController<AdminRoleAssignmentController>
    {
        private readonly IApplicationService applicationService;

        public AdminRoleAssignmentController(IApplicationService _applicationService)
        {
            applicationService = _applicationService;
        }

        // list roles
        [HttpGet("", Name = AdminRoleAssignmentRouteNames.RoleAssignment)]
        public async Task<IActionResult> Index()
        {
            var roles = await applicationService.GetRoles();
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

        [HttpPost("{roleId}", Name= AdminRoleAssignmentRouteNames.HandleAssignmentChoice)]
        public IActionResult HandleAssignmentChoice(int roleId, AssignmentChoiceViewModel model)
        {
            return RedirectToAction(model.Action.ToString(), new { roleId = roleId });
        }

        // list unassigned applications for role id
        [HttpGet("{roleId}/add", Name= AdminRoleAssignmentRouteNames.GetUnassignedApplications)]
        public async Task<IActionResult> GetUnassignedApplications(int roleId)
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
            return View(new ApplicationsForRoleViewModel
            {
                RoleId = roleId,
                Applications = applications
            });
        }

        [HttpPost("{roleId}/remove",Name = AdminRoleAssignmentRouteNames.RemoveApplication)]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveApplicationFromRole(ApplicationsForRoleViewModel model)
        {
            applicationService.RemoveApplicationFromRole(model.ApplicationId, model.RoleId);
            TempData.Put("model", new { Message = "The requested role assignment has been updated." });
            return RedirectToAction(nameof(AdminController.AdminActionComplete), typeof(AdminController));
        }
    }
}
