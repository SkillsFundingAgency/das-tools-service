using System;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ToolService.Web.Controllers
{
    public class BaseController : Controller
    {
        public RedirectToActionResult RedirectToAction(string actionName, Type controller, object routeValues)
        {
            return base.RedirectToAction(actionName, controller.Name.Replace("Controller", ""), routeValues);
        }
    }
}
