using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Core.IServices;
using SFA.DAS.ToolService.Web.Models;
using SFA.DAS.ToolService.Web.Models.Admin;

namespace SFA.DAS.ToolService.Web.Controllers.Admin
{
    public class AdminApplicationController : Controller
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

            return RedirectToAction(model.Action);
        }

        [HttpGet("admin/manage-applications/add-application")]
        public IActionResult AddApplication()
        {
            return View();
        }

        [HttpPost("admin/manage-applications/add-application")]
        public async Task<IActionResult> AddApplication(AddApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                Uri.TryCreate(model.Path, UriKind.RelativeOrAbsolute, out var outUri);
                await _applicationService.AddApplication(model.Name, model.Description, model.Path, outUri.IsAbsoluteUri);
                return RedirectToAction("ActionComplete");
            }
            return View();
        }

        [HttpGet("admin/manage-applications/add-application/complete")]
        public IActionResult ActionComplete()
        {

            var model = new ActionCompleteViewModel
            {
                Message = "The requested application has been added."
            };

            return View(model);
        }
    }
}
