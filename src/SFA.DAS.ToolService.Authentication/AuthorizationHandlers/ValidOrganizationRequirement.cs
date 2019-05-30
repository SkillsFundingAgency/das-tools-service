using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.ToolService.Authentication.AuthorizationHandlers
{
    public class ValidOrganizationRequirement : IAuthorizationRequirement
    {
        public string[] Organizations { get; private set; }

        public ValidOrganizationRequirement(string organizations)
        {
            Organizations = organizations.Split(",");
        }
    }
}
