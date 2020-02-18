using System;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolService.Web.Extensions;

namespace SFA.DAS.ToolService.Web.Controllers
{
    public class BaseController : Controller
    {
        public RedirectToActionResult RedirectToAction(string actionName, Type controller, object routeValues)
        {
            return base.RedirectToAction(actionName, controller.GetControllerName(), routeValues);
        }
    }
}
