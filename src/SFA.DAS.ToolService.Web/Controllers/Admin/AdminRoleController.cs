using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolService.Core.IServices;
using SFA.DAS.ToolService.Web.Configuration;
using SFA.DAS.ToolService.Web.Extensions;
using SFA.DAS.ToolService.Web.Models.Admin;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Web.Controllers.Admin
{
    [Authorize(Policy = "admin")]
    [Route("admin/manage-roles")]
    public class AdminRoleController : BaseController<AdminRoleController>
    {
        private readonly IRoleService roleService;

        public AdminRoleController(IRoleService _roleService)
        {
            roleService = _roleService;
        }

        [HttpGet("", Name = AdminRoleRouteNames.ManageRoles)]
        public async Task<IActionResult> Index()
        {
            var identiyProviderRoles = await roleService.GetExternalRoles();
            var model = new ManageRolesViewModel
            {
                IdentiyProviderRoles = identiyProviderRoles
            };
            return View(model);
        }

        [HttpPost("", Name = AdminRoleRouteNames.SyncRoles)]
        public async Task<IActionResult> SyncRoles(ManageRolesViewModel model)
        {
            await roleService.SyncIdentityProviderRoles();
            TempData.Put("model", new { Message = "Role sync has been started." });
            return RedirectToAction(nameof(AdminController.AdminActionComplete), typeof(AdminController));
        }
    }
}
