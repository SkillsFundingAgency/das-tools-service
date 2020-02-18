using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Core.IServices;

namespace SFA.DAS.ToolService.Web.Controllers
{
    public class AdminApplicationController : Controller
    {
        private readonly ILogger logger;
        private readonly IApplicationService applicationService;

        public AdminApplicationController(ILogger<AdminApplicationController> _logger,
            IApplicationService _applicationService)
        {
            logger = _logger;
            applicationService = _applicationService;
        }

        [HttpGet("admin/manage-applications")]
        public IActionResult ManageApplications()
        {
            return View();
        }
    }
}
