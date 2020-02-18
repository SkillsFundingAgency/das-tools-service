using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ToolService.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Web.Infrastructure
{
    public class AdminUserAuthorizationHandler : AuthorizationHandler<AdminUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminUserRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                return Task.CompletedTask;
            }

            if (context.User.IsInRole(ApplicationConstants.AdminRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
