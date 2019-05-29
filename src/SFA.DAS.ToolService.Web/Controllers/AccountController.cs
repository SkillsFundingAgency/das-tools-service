using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Web.Models;

namespace SFA.DAS.ToolService.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger logger;
        
        public AccountController(ILogger<AccountController> _logger)
        {
            logger = _logger;
        }

        public IActionResult Signin(string returnUrl = "/Home") {
            return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
