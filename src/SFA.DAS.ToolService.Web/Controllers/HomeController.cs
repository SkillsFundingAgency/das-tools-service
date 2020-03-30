using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SFA.DAS.ToolService.Web.Models;

namespace SFA.DAS.ToolService.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController<HomeController>
    {
        private readonly IConfiguration configuration;

        public HomeController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel
            {
                DfeSignInAddress = configuration["DfeSignInAddress"]
            };

            return View(model);
        }
    }
}
