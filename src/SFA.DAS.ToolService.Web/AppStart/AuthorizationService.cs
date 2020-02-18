using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ToolService.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Web.AppStart
{
    public static class AuthorizationService
    {
        public static void AddAuthorizationService(this IServiceCollection services)
        {

            services.AddAuthorization(options =>
                options.AddPolicy("Admin", policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim(ClaimTypes.Role);
                        policy.Requirements.Add(new AdminUserRequirement());
                    })
            );
        }


    }
}
