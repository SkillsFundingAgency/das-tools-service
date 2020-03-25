using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using SFA.DAS.ToolService.Core;
using SFA.DAS.ToolService.Core.Configuration;
using SFA.DAS.ToolService.Core.IRepositories;
using SFA.DAS.ToolService.Web.AppStart;
using System;
using System.IO;

namespace SFA.DAS.ToolService.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            _environment = environment;
            var config = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile("appsettings.Development.json", true)
                .AddEnvironmentVariables()
                .Build();

            _configuration = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = false;
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddServices(_configuration);

            services.AddDbContext<ToolServiceDbContext>(options =>
                options.UseSqlServer(_configuration["DatabaseConnectionString"]));

            services.AddScoped<IToolServiceDbContext, ToolServiceDbContext>(provider => provider.GetService<ToolServiceDbContext>());
            services.AddTransient(provider => new Lazy<ToolServiceDbContext>(provider.GetService<ToolServiceDbContext>()));

            services.AddOptions();

            services.AddAntiforgery(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            var serviceProvider = services.BuildServiceProvider();
            services.AddAuthorizationService();

            services.AddAuth0Authentication(serviceProvider.GetService<IOptions<AuthenticationConfiguration>>());

            services.AddHealthChecks();

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddControllersAsServices()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

            services.AddDistributedCache(_configuration, _environment);

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.IsEssential = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts(options =>
                {
                    options.MaxAge(days: 30);
                    options.IncludeSubdomains();
                });
            }

            app.UseXContentTypeOptions();
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXfo(options => options.SameOrigin());
            app.UseReferrerPolicy(opts => opts.NoReferrerWhenDowngrade());

            var cdnHost = new UriBuilder(_configuration["Cdn:Url"]).Host;
            app.UseCsp(options => options
                .DefaultSources(s => s.Self()
                    .CustomSources("data:")
                    .CustomSources("https:"))
                .StyleSources(s => s.Self()
                    .CustomSources(cdnHost)
                    .UnsafeInline()
                )
                .ScriptSources(s => s.Self()
                       .CustomSources("ajax.googleapis.com", cdnHost)
                    .UnsafeInline()
                    .UnsafeEval()
                )
            );

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.Use(async (context, next) =>
            {
                if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
                {
                    //Re-execute the request so the user gets the error page
                    var originalPath = context.Request.Path.Value;
                    context.Items["originalPath"] = originalPath;
                    context.Request.Path = "/error/404";
                    await next();
                }
            });

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
