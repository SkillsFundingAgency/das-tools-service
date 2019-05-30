using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.ToolService.Authentication.Entities;
using SFA.DAS.ToolService.Web.Models;

namespace SFA.DAS.ToolService.Web.Controllers
{
    public class AccessDeniedController : Controller
    {
        private readonly ILogger logger;
        private readonly AuthenticationConfigurationEntity configuration;
        public AccessDeniedController(ILogger<AccessDeniedController> _logger, IOptions<AuthenticationConfigurationEntity> _configuration)
        {
            logger = _logger;
            configuration = _configuration.Value;
        }

        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {

            logger.LogInformation($"{configuration.GitHub.ClientId}");

            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "urn:github:login" && c.Issuer == "GitHub").Value;
            logger.LogInformation($"The user {user} is not authorized to access this service. Ensure that they are a member of the correct organization.");
            return View("AccessDenied", new AccessDeniedViewModel(configuration.GitHub.ValidOrganizations));
        }
    }
}