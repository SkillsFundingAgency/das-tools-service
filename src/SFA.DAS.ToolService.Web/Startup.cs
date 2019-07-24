using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Authentication.ServiceCollectionExtensions;
using SFA.DAS.ToolService.Authentication.Entities;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.StackExchangeRedis;
using StackExchange.Redis;

namespace SFA.DAS.ToolService.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        private readonly ILogger _logger;
        public Startup(IConfiguration configuration, IHostingEnvironment env, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _env = env;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationOptions = Configuration.GetSection("Authentication");

            services.Configure<AuthenticationConfigurationEntity>(authenticationOptions);

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                    ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            var redisConnectionString = Configuration["RedisConnectionString"];
            var redis = ConnectionMultiplexer.Connect($"{redisConnectionString},DefaultDatabase=0");
            services.AddDataProtection()
                .SetApplicationName("das-tools-service")
                .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAntiforgery(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            services.AddHealthChecks();

            services.AddAuthenticationProviders(authenticationOptions.Get<AuthenticationConfigurationEntity>());

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {

            app.UseForwardedHeaders();
            app.Use(async (context, next) =>
            {
                // Request method, scheme, and path
                _logger.LogDebug("Request Method: {METHOD}", context.Request.Method);
                _logger.LogDebug("Request Scheme: {SCHEME}", context.Request.Scheme);
                _logger.LogDebug("Request Path: {PATH}", context.Request.Path);

                // Headers
                foreach (var header in context.Request.Headers)
                {
                    _logger.LogDebug("Header: {KEY}: {VALUE}", header.Key, header.Value);
                }

                // Connection: RemoteIp
                _logger.LogDebug("Request RemoteIp: {REMOTE_IP_ADDRESS}",
                    context.Connection.RemoteIpAddress);

                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Xss-Protection", "1");
                await next();
            });
            // app.Use(async (context, next) =>
            // {
            //     if (context.Request.Headers.ContainsKey("X-Original-Host"))
            //     {
            //         var originalHost = context.Request.Headers["X-Original-Host"];
            //         logger.LogInformation($"Retrieving X-Original-Host value {originalHost}");
            //         context.Request.Headers.Add("Host", originalHost);
            //     }
            //     await next.Invoke();
            // });

            if (env.IsDevelopment())
            {
                logger.LogInformation($"App is running in development mode: {env.EnvironmentName}");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                logger.LogInformation($"App is running in production mode: {env.EnvironmentName}");
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseHealthChecks("/health");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
