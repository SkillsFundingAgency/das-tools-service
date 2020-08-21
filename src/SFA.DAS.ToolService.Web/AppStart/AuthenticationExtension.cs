using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SFA.DAS.ToolService.Core.Configuration;
using SFA.DAS.ToolService.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Web.AppStart
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services, IOptions<AuthenticationConfiguration> configuration, IOptions<GitHub> githubconfiguration)
        {
            services.AddSingleton<IAuthorizationHandler, ValidGitHubRequirementsHandler>();

            // Add an authorization policy to check whether our username is part of defined GitHub orgs and teams
            // need to pass in GitHub array from config incase there are multiple GitHub organisations
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("ValidGitHubOrgsAndTeamsOnly", policy => policy.Requirements.Add(
            //        new ValidGitHubRequirement(githubconfiguration.Value.Organisation)
            //        )
            //    );
            //});

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
                options.Authority = $"https://{configuration.Value.Domain}/realms/PoC";
                options.ClientId = configuration.Value.ClientId;
                options.ClientSecret = configuration.Value.ClientSecret;
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
