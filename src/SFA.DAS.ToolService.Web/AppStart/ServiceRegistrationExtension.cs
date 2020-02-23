using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.ToolService.Core.Configuration;
using SFA.DAS.ToolService.Core.IRepositories;
using SFA.DAS.ToolService.Core.IServices;
using SFA.DAS.ToolService.Core.Services;
using SFA.DAS.ToolService.Infrastructure.Repositories;
using SFA.DAS.ToolService.Infrastructure.Auth0;
using SFA.DAS.ToolService.Web.Infrastructure;
using Auth0.ManagementApi;

namespace SFA.DAS.ToolService.Web.AppStart
{
    public static class ServiceRegistrationExtension
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthenticationConfiguration>(configuration.GetSection("Authentication"));
            services.AddSingleton(config => config.GetService<IOptions<AuthenticationConfiguration>>().Value);
            services.AddScoped<IApplicationService, ApplicationService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddSingleton<IManagementConnection, HttpClientManagementConnection>();
            services.AddScoped<IApiClient, ApiClient>();
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IAuth0RoleRepository, Auth0RoleRepository>();
            services.AddSingleton<IAuthorizationHandler, AdminUserAuthorizationHandler>();
            services.AddRouting(options => options.LowercaseUrls = true);
        }
    }
}
