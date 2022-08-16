using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolService.Core.IServices;
using SFA.DAS.ToolService.Web.Configuration;
using SFA.DAS.ToolService.Web.Extensions;
using SFA.DAS.ToolService.Web.Models.Admin;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Web.Controllers.Admin
{
    [Authorize(Policy = "admin")]
    [Route("admin/manage-applications")]
    public class AdminApplicationController : BaseController<AdminApplicationController>
    {
        private readonly IApplicationService _applicationService;

        public AdminApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet("", Name = AdminApplicationRouteNames.ManageApplications)]
        public IActionResult Index()
        {
            return View(new ManageApplicationsViewModel());
        }

        [HttpPost("")]
        public IActionResult ManageApplicationsHandleChoice(ManageApplicationsViewModel model)
        {
            return RedirectToAction(model.Action.ToString());
        }

        [HttpGet("add", Name = AdminApplicationRouteNames.AddNewApplication)]
        public IActionResult AddApplication()
        {
            return View();
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddApplication(AddApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                Uri.TryCreate(model.Path, UriKind.RelativeOrAbsolute, out var outUri);
                await _applicationService.AddApplication(model.Name, model.Description, model.Path, outUri.IsAbsoluteUri);
                TempData.Put("model", new { Message = "The requested application has been added." });
                return RedirectToAction(nameof(AdminController.AdminActionComplete), typeof(AdminController));
            }
            return View();
        }

        [HttpGet("remove")]
        public async Task<IActionResult> RemoveApplication()
        {
            var existingApplications = await _applicationService.GetUnassignedApplications();
            return View(new RemoveApplicationViewModel { ExistingApplications = existingApplications });
        }

        [HttpPost("remove")]
        public IActionResult RemoveApplication(RemoveApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                _applicationService.RemoveApplication(model.SelectedApplication);
                TempData.Put("model", new { Message = "The requested application has been removed." });
                return RedirectToAction(nameof(AdminController.AdminActionComplete), typeof(AdminController));
            }
            return View(model);
        }
    }
}
