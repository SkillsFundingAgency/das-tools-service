using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.ToolService.Authentication.AuthorizationHandlers
{
    public class HasRoleRequirement : IAuthorizationRequirement
    {
        public string[] Roles { get; set; }

        public HasRoleRequirement(string[] roles)
        {
            Roles = roles;
        }
    }
}
