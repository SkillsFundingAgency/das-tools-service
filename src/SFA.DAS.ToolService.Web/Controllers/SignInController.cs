using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.ToolService.Core.Configuration;
using SFA.DAS.ToolService.Web.Models;

namespace SFA.DAS.ToolService.Web.Controllers
{
    [AllowAnonymous]
    public class SignInController : BaseController<SignInController>
    {

        public SignInController()
        {
        }

        public IActionResult Index()
        {
            var model = new SignInViewModel();

            return View(model);
        }
    }
}
