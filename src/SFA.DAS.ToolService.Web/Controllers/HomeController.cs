using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Web.Models;

namespace SFA.DAS.ToolService.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger logger;
        public HomeController(ILogger<HomeController> _logger)
        {
            logger = _logger;
        }

        public IActionResult Index()
        {
            logger.LogDebug("Request Method: {METHOD}", HttpContext.Request.Method);
            logger.LogDebug("Request Scheme: {SCHEME}", HttpContext.Request.Scheme);
            logger.LogDebug("Request Path: {PATH}", HttpContext.Request.Path);

            // Headers
            foreach (var header in HttpContext.Request.Headers)
            {
                logger.LogDebug("Header: {KEY}: {VALUE}", header.Key, header.Value);
            }

            // Connection: RemoteIp
            logger.LogDebug("Request RemoteIp: {REMOTE_IP_ADDRESS}",
                HttpContext.Connection.RemoteIpAddress);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}