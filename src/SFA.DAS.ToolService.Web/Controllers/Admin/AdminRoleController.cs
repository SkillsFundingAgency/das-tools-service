using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolService.Core.IServices;
using SFA.DAS.ToolService.Web.Configuration;
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
            var localRoles = await roleService.GetRoles();
            var model = new ManageRolesViewModel
            {
                LocalRoles = localRoles
            };
            return View(model);
        }
    }
}
