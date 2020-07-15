using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Web.Controllers
{
    public class AccountController : BaseController<AccountController>
    {
        public AccountController()
        {
        }

        public async Task Login(string returnUrl = "/home")
        {
            await HttpContext.ChallengeAsync("Keycloak", new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Keycloak", new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index", "Home")
            });
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
