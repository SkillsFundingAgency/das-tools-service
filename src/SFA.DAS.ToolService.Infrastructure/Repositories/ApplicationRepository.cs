using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Core;
using SFA.DAS.ToolService.Core.Entities;
using SFA.DAS.ToolService.Core.IRepositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Infrastructure.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly IToolServiceDbContext _toolServiceDbContext;
        private readonly ILogger<ToolServiceDbContext> _logger;

        public ApplicationRepository(IToolServiceDbContext toolServiceDbContext, ILogger<ToolServiceDbContext> logger)
        {
            _toolServiceDbContext = toolServiceDbContext;
            _logger = logger;
        }

        public async Task<List<Application>> GetApplicationsByRoleName(string name)
        {
            var result = await _toolServiceDbContext.ApplicationRole
                .Where(c => c.Role.Name.Equals(name))
                .Select(c => c.Application).ToListAsync();
            return result;
        }

        public async Task<List<Application>> GetApplicationsInRole(int id)
        {
            var result = await _toolServiceDbContext.ApplicationRole
                .Where(c => c.RoleId.Equals(id) & c.Application.Public.Equals(0))
                .Select(c => c.Application).ToListAsync();

            return result;
        }

        public async Task<List<Application>> GetApplicationsNotInRole(int id)
        {
            var roleMappings = await _toolServiceDbContext.ApplicationRole
                .Where(c => c.RoleId.Equals(id))
                .Select(c => c.ApplicationId).ToArrayAsync();

            var result = await _toolServiceDbContext.Application
                .Where(c => !roleMappings.Contains(c.Id) & c.Public == 0).ToListAsync();

            return result;
        }

        public async Task<List<Application>> GetApplicationsWithNoRoleAssignment()
        {
            var roleMappings = await _toolServiceDbContext.ApplicationRole
                .Select(c => c.ApplicationId).ToArrayAsync();

            var result = await _toolServiceDbContext.Application
                .Where(c => !roleMappings.Contains(c.Id) & c.Public == 0).ToListAsync();

            return result;
        }

        public async Task<List<Application>> GetPublicApplications()
        {
            var result = await _toolServiceDbContext.Application
                .Where(c => c.Public.Equals(1)).ToListAsync();

            return result;
        }

        public async Task<List<Application>> GetApplications()
        {
            var result = await _toolServiceDbContext.Application.ToListAsync();

            return result;
        }

        public async Task<List<Role>> GetRoles()
        {
            var result = await _toolServiceDbContext.Role.ToListAsync();

            return result;
        }

        public async Task<int> GetRoleId(string name)
        {
            var result = await _toolServiceDbContext.Role
                .Where(c => c.Name.Equals(name))
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            _logger.LogInformation($"Got id {result} for role {name}");

            return result;
        }

        public async Task InsertApplicationRoleMapping(int applicationId, int roleId)
        {
            var applicationRoleMapping = new ApplicationRole
            {
                ApplicationId = applicationId,
                RoleId = roleId
            };

            await _toolServiceDbContext.ApplicationRole
                .AddAsync(applicationRoleMapping);

            _toolServiceDbContext.SaveChanges();
        }

        public void DeleteApplicationRoleMapping(int applicationId, int roleId)
        {
            var mappingId = _toolServiceDbContext.ApplicationRole
                .Where(c => c.ApplicationId.Equals(applicationId) && c.RoleId.Equals(roleId)).FirstOrDefault();

            _toolServiceDbContext.ApplicationRole
                .Remove(mappingId);

            _toolServiceDbContext.SaveChanges();
        }

        public async Task AddApplication(Application application)
        {
            await _toolServiceDbContext.Application.AddAsync(application);

            _toolServiceDbContext.SaveChanges();
        }

        public void RemoveApplication(int id)
        {
            var app = _toolServiceDbContext.Application.First(a => a.Id == id);
            var appRoles = _toolServiceDbContext.ApplicationRole.Where(r => r.ApplicationId == app.Id);
            _toolServiceDbContext.ApplicationRole.RemoveRange(appRoles);
            _toolServiceDbContext.Application.Remove(app);
            _toolServiceDbContext.SaveChanges();
        }
    }
}
