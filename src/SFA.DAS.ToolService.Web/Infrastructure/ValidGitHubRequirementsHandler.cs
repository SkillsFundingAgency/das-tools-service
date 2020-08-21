using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace SFA.DAS.ToolService.Web.Infrastructure
{
    public class ValidGitHubRequirementsHandler : AuthorizationHandler<ValidGitHubRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       ValidGitHubRequirement requirement)
        {
            // Change to check whether context response from Keycloak is originally from GitHub
            if (!context.User.HasClaim(c => c.Type == "urn:github:orgs" &&
                                            c.Issuer == "GitHub"))
            {
                return Task.CompletedTask;
            }

            // Create a client so we can query GitHub API endpoints
            var client = new GitHubClient(new ProductHeaderValue("das-tools-service"));

            // Get the token from the context used by Keycloak
            Credentials tokenAuth = new Credentials("needs passing in as it's not part of AuthorizationHandlerContext");
            client.Credentials = tokenAuth;

            // Get GitHub username from Keycloak claim httpcontext.user.identity.claims look for type = preferred_username, value = currentusersgitusername
            var githubusername = context.User.Identity.Name;

            // Get user organisations if they match any from GitHub settings then test for teams
            var userOrganisations = client.Organization.GetAllForUser(githubusername);

            // If user is part of the required org then test if they are in the required teams specified

            // Get team membership using team id from GitHub object

            //foreach(Team in GitHubTeamIdList)
            //{
            //    IEnumerable<string> teamMembers = client.Organization.Team.GetAllMembers("teamidinteger");

            //    if (teamMembers.Contains(githubusername) == true)
            //    {
            //        context.Succeed(requirement);
            //    }
            //}

            return Task.CompletedTask;
        }
    }
}
