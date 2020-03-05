using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ToolService.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController<HomeController>
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
