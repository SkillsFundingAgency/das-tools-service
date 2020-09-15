using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Web.Infrastructure
{
    public class ValidGitHubRequirementsHandler : AuthorizationHandler<ValidGitHubRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       ValidGitHubRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "provider" &&
                                            c.Value == "github"))
            {
                if(context.User.HasClaim(c => c.Type == "provider" && c.Value == "aad"))
                {
                    context.Succeed(requirement);
                }
                return Task.CompletedTask;
            }

            var organizations = context.User.FindFirst(c => c.Type == "orgs_list").Value.Split(",");

            if (organizations.Intersect(requirement.Organizations).Any())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
