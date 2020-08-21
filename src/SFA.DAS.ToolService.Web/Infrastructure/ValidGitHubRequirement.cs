using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.ToolService.Web.Infrastructure
{
    public class ValidGitHubRequirement : IAuthorizationRequirement
    {
        public string[] Organizations { get; private set; }

        // Need to add some elements in for the teams array
        public ValidGitHubRequirement(string organizations)
        {
            Organizations = organizations.Split(",");
        }
    }
}
