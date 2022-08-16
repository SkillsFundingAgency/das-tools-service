using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Web.Extensions;
using System;

namespace SFA.DAS.ToolService.Web.Controllers
{
    public abstract class BaseController<T> : Controller where T : BaseController<T>
    {

        public RedirectToActionResult RedirectToAction(string actionName, Type controller, object routeValues)
        {
            return base.RedirectToAction(actionName, controller.GetControllerName(), routeValues);
        }

        public RedirectToActionResult RedirectToAction(string actionName, Type controller)
        {
            return base.RedirectToAction(actionName, controller.GetControllerName());
        }
    }
}
