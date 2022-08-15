using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolService.Web.Models;
using SFA.DAS.ToolService.Web.Models.Admin;

namespace SFA.DAS.ToolService.Web.Controllers.Admin
{
    [Authorize(Policy = "admin")]
    [Route("admin")]
    public class AdminController : Controller
    {
        public AdminController()
        {
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View("Index", new IndexViewModel());
        }

        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public IActionResult IndexHandleChoice(IndexViewModel model)
        {
            if (string.IsNullOrEmpty(model.Choice))
            {
                return new BadRequestResult();
            }

            return RedirectToRoute(model.Choice);
        }

        [HttpGet("complete")]
        public IActionResult AdminActionComplete()
        {
            var model = Extensions.Extensions.Get<AdminActionCompleteViewModel>(TempData, "model");
            return View(model);
        }
    }
}
