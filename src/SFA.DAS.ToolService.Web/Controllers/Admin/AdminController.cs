using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Core.IServices;
using SFA.DAS.ToolService.Web.Models;

namespace SFA.DAS.ToolService.Web.Controllers.Admin
{
    [Authorize(Policy = "admin")]
    public class AdminController : BaseController
    {
        private readonly ILogger logger;
        private readonly IApplicationService applicationService;

        public AdminController(ILogger<AdminController> _logger,
            IApplicationService _applicationService)
        {
            logger = _logger;
            applicationService = _applicationService;
        }

        [HttpGet("admin")]
        public IActionResult Index()
        {
            return View("Index", new IndexViewModel());
        }

        [HttpPost("admin")]
        [ValidateAntiForgeryToken]
        public IActionResult IndexHandleChoice(IndexViewModel model)
        {

            if (string.IsNullOrEmpty(model.Choice))
            {
                return new BadRequestResult();
            }

            return RedirectToRoute(model.Choice);
        }

        [HttpGet("admin/complete")]
        public IActionResult ActionComplete(string message)
        {

            var model = new ActionCompleteViewModel
            {
                Message = message
            };

            return View(model);
        }
    }
}
