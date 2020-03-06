using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Core.Entities;
using SFA.DAS.ToolService.Core.IRepositories;
using SFA.DAS.ToolService.Core.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Core.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly ILogger<ApplicationService> _logger;
        private readonly IApplicationRepository _applicationRepository;

        public ApplicationService(ILogger<ApplicationService> logger, IApplicationRepository applicationRepository)
        {
            _logger = logger;
            _applicationRepository = applicationRepository;
        }

        public async Task<List<Application>> GetApplicationsForRole(string[] roles)
        {
            var applications = new List<Application>();
            var publicApplications = await _applicationRepository.GetPublicApplications();
            applications.AddRange(publicApplications);

            foreach (var role in roles)
            {
                var app = await _applicationRepository.GetApplicationsByRoleName(role);
                applications.AddRange(app);
            }

            // Distinct to remove duplicate apps from if in more than one role
            return applications.Distinct().OrderBy(c => c.Name).ToList();
        }

        public async Task<List<Application>> GetApplicationsForRoleId(int roleId)
        {
            var applications = await _applicationRepository.GetApplicationsInRole(roleId);
            return applications.OrderBy(c => c.Name).ToList();
        }

        public async Task<List<Application>> GetUnassignedApplications()
        {
            var applications = await _applicationRepository.GetApplicationsWithNoRoleAssignment();
            return applications.OrderBy(c => c.Name).ToList();
        }

        public async Task<List<Application>> GetUnassignedApplicationsForRole(int id)
        {
            var applications = await _applicationRepository.GetApplicationsNotInRole(id);
            return applications.OrderBy(c => c.Name).ToList();
        }

        public async Task AssignApplicationToRole(int applicationId, int roleId)
        {
            await _applicationRepository.InsertApplicationRoleMapping(applicationId, roleId);
        }

        public void RemoveApplicationFromRole(int applicationId, int roleId)
        {
            _applicationRepository.DeleteApplicationRoleMapping(applicationId, roleId);
        }

        public async Task AddApplication(string name, string description, string path, bool isExternal)
        {
            var application = new Application()
            {
                Name = name,
                Description = description,
                Path = path,
                IsExternal = isExternal ? 1 : 0
            };
            await _applicationRepository.AddApplication(application);
        }

        public async Task<List<Application>> GetAllApplications()
        {
            var applications = await _applicationRepository.GetApplications();
            return applications.OrderBy(c => c.Name).ToList();
        }

        public void RemoveApplication(int id)
        {
            _applicationRepository.RemoveApplication(id);
        }
    }
}
