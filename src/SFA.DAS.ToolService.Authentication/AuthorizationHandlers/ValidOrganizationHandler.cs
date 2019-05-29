using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

public class ValidOrganizationHandler : AuthorizationHandler<ValidOrganizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   ValidOrganizationRequirement requirement)
    {
        if (!context.User.HasClaim(c => c.Type == "urn:github:orgs" &&
                                        c.Issuer == "GitHub"))
        {
            return Task.CompletedTask;
        }

        var organizations =
            context.User.FindFirst(c => c.Type == "urn:github:orgs" &&
                                        c.Issuer == "GitHub").Value.Split(",");

        if (organizations.Intersect(requirement.Organizations).Any())
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}