using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Octokit;
using RestSharp;
using SFA.DAS.ToolService.Core.Configuration;
using SFA.DAS.ToolService.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace SFA.DAS.ToolService.Web.AppStart
{
    public static class AuthenticationExtension
    {
        private static ILogger<Startup> _logger;

        public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services, IOptions<AuthenticationConfiguration> configuration, ILogger<Startup> logger)
        {
            _logger = logger;

            services.AddSingleton<IAuthorizationHandler, ValidGitHubRequirementsHandler>();

            // Add an authorization policy to check whether our username is part of defined GitHub orgs and teams
            // need to pass in GitHub array from config incase there are multiple GitHub organisations
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ValidGitHubOrgsAndTeams", policy => policy.Requirements.Add(
                    new ValidGitHubRequirement(configuration.Value.GitHub)
                    )
                );
            });

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
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority = $"https://{configuration.Value.Domain}/realms/{configuration.Value.Realm}";
                options.ClientId = configuration.Value.ClientId;
                options.ClientSecret = configuration.Value.ClientSecret;
                options.ClaimsIssuer = "Keycloak";
                options.ResponseType = "code";
                options.RequireHttpsMetadata = false;

                options.Events = new OpenIdConnectEvents
                {
                    OnTokenValidated = async ctx =>
                    {
                        var provider = ctx.Principal.Claims.FirstOrDefault(x => x.Type == "provider");
                        if (provider != null && provider.Value == "github")
                        {
                            var gitToken = await GetGithubTokenFromKeyCloakAsync(ctx, configuration);

                            if (!string.IsNullOrEmpty(gitToken))
                            {
                                var githubMetaData = await GetGithubMetaDataClaims(gitToken);

                                var newClaims = new ClaimsIdentity(githubMetaData);

                                ctx.Principal.AddIdentity(newClaims);
                            }
                            else
                            {
                                logger.LogInformation("Unable to obtain github token");
                            }
                        }
                    }
                };
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

        private static async Task<string> GetGithubTokenFromKeyCloakAsync(TokenValidatedContext context, IOptions<AuthenticationConfiguration> configuration)
        {
            var keycloakToken = context.TokenEndpointResponse.AccessToken;

            if (string.IsNullOrEmpty(keycloakToken))
            {
                _logger.LogInformation("Unable to option keycloak token");
                return null;
            }
            else
            {
                var client = new RestClient($"https://{configuration.Value.Domain}/realms/{configuration.Value.Realm}/broker/github/token");

                var request = new RestRequest(Method.GET);

                request.AddHeader("Authorization", $"Bearer {keycloakToken}");

                var response = await client.ExecuteAsync(request);

                var query = HttpUtility.ParseQueryString(response.Content);

                var gitToken = query["access_token"];

                return gitToken;
            }
        }

        private static async Task<List<Claim>> GetGithubMetaDataClaims(string githubToken)
        {
            var github = new GitHubClient(new Octokit.ProductHeaderValue("tools-service"))
            {
                Credentials = new Credentials(githubToken)
            };

            var orgs = await github.Organization.GetAllForCurrent();

            var orgList = string.Join(",", orgs.Select(o => o.Id));

            var teams = await github.Organization.Team.GetAllForCurrent();

            var teamList = string.Join(",", teams.Select(t => t.Name));

            var metaData = new List<Claim>
            {
                new Claim("orgs_list", orgList),

                new Claim("teams_list", teamList)
            };

            return metaData;
        }
    }
}
