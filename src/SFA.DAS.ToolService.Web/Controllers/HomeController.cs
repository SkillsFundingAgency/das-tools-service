using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ToolService.Web.Controllers
{
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
