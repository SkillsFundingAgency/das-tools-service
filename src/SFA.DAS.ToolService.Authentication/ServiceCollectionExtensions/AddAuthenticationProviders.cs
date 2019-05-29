﻿using System;
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
using Newtonsoft.Json.Linq;

namespace SFA.DAS.ToolService.Authentication.ServiceCollectionExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthenticationProviders(this IServiceCollection services, IConfiguration configuration)
        {

            var authenticationConfiguration = configuration.GetSection("Authentication:GitHub");
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ValidOrgsOnly", policy => policy.Requirements.Add(
                    new ValidOrganizationRequirement(
                        authenticationConfiguration.GetValue<string>("ValidOrganizations")
                        )
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
                            options.ReturnUrlParameter = "/";
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
                        options.ClientId = authenticationConfiguration.GetValue<string>("ClientId");
                        options.ClientSecret = authenticationConfiguration.GetValue<string>("ClientSecret");
                        options.CallbackPath = new PathString(authenticationConfiguration.GetValue<string>("CallbackPath"));
                        options.Scope.Add(authenticationConfiguration.GetValue<string>("Scope"));

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
    }
}
