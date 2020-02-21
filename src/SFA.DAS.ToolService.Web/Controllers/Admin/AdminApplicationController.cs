using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Core.IServices;
using SFA.DAS.ToolService.Web.Models.Admin;
using SFA.DAS.ToolService.Web.Extensions;

namespace SFA.DAS.ToolService.Web.Controllers.Admin
{
    public class AdminApplicationController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IApplicationService _applicationService;

        public AdminApplicationController(ILogger<AdminApplicationController> logger,
            IApplicationService applicationService)
        {
            _logger = logger;
            _applicationService = applicationService;
        }

        [HttpGet("admin/manage-applications", Name = "ManageApplications")]
        public IActionResult ManageApplications()
        {
            return View(new ManageApplicationsViewModel());
        }

        [HttpPost("admin/manage-applications")]
        public IActionResult ManageApplicationsHandleChoice(ManageApplicationsViewModel model)
        {
            return RedirectToAction(model.Action.ToString());
        }

        [HttpGet("admin/manage-applications/add")]
        public IActionResult AddApplication()
        {
            return View();
        }

        [HttpPost("admin/manage-applications/add")]
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

        [HttpGet("admin/manage-applications/remove")]
        public async Task<IActionResult> RemoveApplication()
        {
            var existingApplications = await _applicationService.GetAllApplications();
            return View(new RemoveApplicationViewModel { ExistingApplications = existingApplications });
        }
        
        [HttpPost("admin/manage-applications/remove")]
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
