using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SFA.DAS.ToolService.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Web.AppStart
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services, IOptions<AuthenticationConfiguration> configuration)
        {
            // Add authentication services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "Keycloak";
                options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddOpenIdConnect("Keycloak", options =>
            {
                options.Authority = $"https://dev-tools.apprenticeships.education.gov.uk/auth/realms/PoC";
                options.ClientId = "poc";
                options.ClientSecret = "d517e806-a205-4246-b425-ed68cf38381c";
                options.ClaimsIssuer = "Keycloak";
                options.ResponseType = "code";
            })
            .AddCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Error/403");
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.Cookie.Name = "SFA.DAS.ToolService.Web.Auth";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.SlidingExpiration = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.CookieManager = new ChunkingCookieManager() { ChunkSize = 3000 };
            });

            return services;
        }
    }
}
