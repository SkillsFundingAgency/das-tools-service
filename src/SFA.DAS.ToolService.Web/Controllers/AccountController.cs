using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        [Route("SignIn")]
        public IActionResult SignIn()
        {
            return RedirectToAction("Account");
        }

        [Route("SignOut")]
        public IActionResult SignOut()
        {
            return RedirectToAction("Index");
        }

        [Route("Home")]
        public IActionResult Account()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
