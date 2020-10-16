using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolService.Core.IServices;
using SFA.DAS.ToolService.Web.Configuration;
using SFA.DAS.ToolService.Web.Extensions;
using SFA.DAS.ToolService.Web.Models.Admin;
using SFA.DAS.ToolsNotifications.Types.Entities;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Web.Controllers.Admin
{
    [Authorize(Policy = "ValidGitHubOrgsAndTeams")]
    [Authorize(Policy = "admin")]
    [Route("admin/manage-notifications")]
    public class AdminNotificationController : BaseController<AdminNotificationController>
    {
        private readonly INotificationService _notificationService;

        public AdminNotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("", Name = AdminNotificationRouteNames.ManageNotifications)]
        public async Task<IActionResult> Index()
        {
            var currentNotification = await _notificationService.GetNotification();

            var model = new ManageNotificationsViewModel();

            if (currentNotification != null)
            {
                model.Title = currentNotification.Title;
                model.Description = currentNotification.Description;
                model.Enabled = currentNotification.Enabled;
            }

            return View(model);
        }

        [HttpPost("", Name = AdminNotificationRouteNames.ManageNotifications)]
        public async Task<IActionResult> Index(ManageNotificationsViewModel model)
        {
            var notification = new Notification
            {
                Title = model.Title,
                Description = model.Description,
                Enabled = model.Enabled
            };

            await _notificationService.SetNotification(notification);
            TempData.Put("model", new { Message = "Notification banner updated!" });
            return RedirectToAction(nameof(AdminController.AdminActionComplete), typeof(AdminController));
        }
    }
}
