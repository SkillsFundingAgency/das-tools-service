using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.ToolService.Core.Configuration;
using SFA.DAS.ToolService.Web.Models;

namespace SFA.DAS.ToolService.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController<HomeController>
    {
        private readonly IOptions<HomePageLinksConfiguration> links;

        public HomeController(IOptions<HomePageLinksConfiguration> _links)
        {
            links = _links;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel
            {
                GitHubOrgAddress = links.Value.GitHubOrgAddress
            };

            return View(model);
        }
    }
}
