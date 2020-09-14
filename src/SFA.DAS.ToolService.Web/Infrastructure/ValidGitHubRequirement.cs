using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ToolService.Core.Configuration;

namespace SFA.DAS.ToolService.Web.Infrastructure
{
    public class ValidGitHubRequirement : IAuthorizationRequirement
    {
        public string[] Organizations { get; private set; }

        public string[] Teams { get; private set; }

        public ValidGitHubRequirement(GitHub gitHub)
        {
            Organizations = gitHub.Organisation.Split(",");
            Teams = gitHub.Teams.Split(",");
        }
    }
}
