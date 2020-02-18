using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
namespace SFA.DAS.ToolService.Authentication.AuthorizationHandlers
{
    public class HasRoleHandler : AuthorizationHandler<HasRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       HasRoleRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "urn:github:orgs" &&
                                            c.Issuer == "GitHub"))
            {
                return Task.CompletedTask;
            }

            var roles =
                context.User.FindFirst(c => c.Type == "urn:github:orgs" &&
                                            c.Issuer == "GitHub").Value.Split(",");

            if (roles.Intersect(requirement.Roles).Any())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
