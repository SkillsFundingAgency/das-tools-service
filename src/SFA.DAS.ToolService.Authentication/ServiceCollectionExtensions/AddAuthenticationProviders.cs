using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ToolService.Authentication.AuthorizationHandlers;
using SFA.DAS.ToolService.Authentication.Entities;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;

namespace SFA.DAS.ToolService.Authentication.ServiceCollectionExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthenticationProviders(this IServiceCollection services, AuthenticationConfigurationEntity authenticationConfiguration)
        {

            services.AddSingleton<IAuthorizationHandler, ValidOrganizationHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ValidOrgsOnly", policy => policy.Requirements.Add(
                    new ValidOrganizationRequirement(authenticationConfiguration.GitHub.ValidOrganizations)
                    )
                );
            });

            services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = "GitHub";
                    })
                    .AddCookie(options =>
                        {
                            options.Cookie.Name = "tools-auth";
                            options.AccessDeniedPath = "/AccessDenied/";
                            options.ReturnUrlParameter = authenticationConfiguration.GitHub.RedirectUrl;
                            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                            // options.Cookie.SameSite = SameSiteMode.Strict;
                            options.SlidingExpiration = true;
                            options.Events.OnSigningIn = (context) =>
                            {
                                context.CookieOptions.Expires = DateTimeOffset.UtcNow.AddMinutes(30);
                                return Task.CompletedTask;
                            };
                        }
                    )
                    .AddOAuth("GitHub", options =>
                    {
                        options.ClientId = authenticationConfiguration.GitHub.ClientId;
                        options.ClientSecret = authenticationConfiguration.GitHub.ClientSecret;
                        options.CallbackPath = new PathString(authenticationConfiguration.GitHub.CallbackPath);
                        options.Scope.Add(authenticationConfiguration.GitHub.Scope);

                        options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                        options.TokenEndpoint = "https://github.com/login/oauth/access_token";
                        options.UserInformationEndpoint = "https://api.github.com/user";

                        options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                        options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                        options.ClaimActions.MapJsonKey("urn:github:login", "login");
                        options.ClaimActions.MapJsonKey("urn:github:url", "html_url");
                        options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");
                        options.ClaimActions.MapJsonKey("urn:github:orgs", "orgs_list");

                        options.Events = new OAuthEvents
                        {
                            OnCreatingTicket = async context =>
                            {
                                var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                                var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                                response.EnsureSuccessStatusCode();

                                var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                                // Get organisation membership
                                request = new HttpRequestMessage(HttpMethod.Get, $"{options.UserInformationEndpoint}/orgs");
                                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                                response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                                response.EnsureSuccessStatusCode();

                                var organisations = JArray.Parse(await response.Content.ReadAsStringAsync());

                                var organisationNames = organisations.Select(x => x.Value<string>("login"));

                                user.Add("orgs_list", String.Join(",", organisationNames));

                                context.RunClaimActions(user);

                            }
                        };
                    });

            return services;
        }

        public static IServiceCollection AddAuth0(this IServiceCollection services, AuthenticationConfigurationEntity authenticationConfiguration)
        {

            // Add authentication services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect("Auth0", options =>
            {
                // Set the authority to your Auth0 domain
                options.Authority = $"https://{authenticationConfiguration.Auth0.Domain}";

                // Configure the Auth0 Client ID and Client Secret
                options.ClientId = authenticationConfiguration.Auth0.ClientId;
                options.ClientSecret = authenticationConfiguration.Auth0.ClientSecret;

                // Set response type to code
                options.ResponseType = "code";

                // Configure the scope
                options.Scope.Clear();
                options.Scope.Add("openid");

                // Set the callback path, so Auth0 will call back to http://localhost:3000/callback
                // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard
                options.CallbackPath = new PathString("/callback");

                // Configure the Claims Issuer to be Auth0
                options.ClaimsIssuer = "Auth0";

                options.Events = new OpenIdConnectEvents
                {
                    // handle the logout redirection
                    OnRedirectToIdentityProviderForSignOut = (context) =>
                            {
                                var logoutUri = $"https://{authenticationConfiguration.Auth0.Domain}/v2/logout?client_id={authenticationConfiguration.Auth0.ClientId}";

                                var postLogoutUri = context.Properties.RedirectUri;
                                if (!string.IsNullOrEmpty(postLogoutUri))
                                {
                                    if (postLogoutUri.StartsWith("/"))
                                    {
                                        // transform to absolute
                                        var request = context.Request;
                                        postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                                    }
                                    logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
                                }

                                context.Response.Redirect(logoutUri);
                                context.HandleResponse();

                                return Task.CompletedTask;
                            }
                };
            });

            return services;

        }

    }
}
