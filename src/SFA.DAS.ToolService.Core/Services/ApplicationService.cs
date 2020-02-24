using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Core.Entities;
using SFA.DAS.ToolService.Core.IRepositories;
using SFA.DAS.ToolService.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Core.Services
{
    public class ApplicationService: IApplicationService
    {
        private readonly ILogger<ApplicationService> logger;
        private readonly IApplicationRepository applicationRepository;

        public ApplicationService(ILogger<ApplicationService> _logger, IApplicationRepository _applicationRepository)
        {
            logger = _logger;
            applicationRepository = _applicationRepository;
        }

        public async Task<List<Application>> GetApplicationsForRole(string[] roles)
        {

            var applications = new List<Application>();
            var publicApplications = await applicationRepository.GetPublicApplications();
            applications.AddRange(publicApplications);

            foreach (var role in roles)
            {
                var roleId = await applicationRepository.GetRoleId(role);
                var app = await applicationRepository.GetApplicationsInRole(roleId);
                applications.AddRange(app);
            }

            // Distinct to remove duplicate apps from if in more than one role
            return applications.Distinct().OrderBy(c => c.Name).ToList();
        }

        public async Task<List<Application>> GetApplicationsForRoleId(int roleId)
        {
            var applications = await applicationRepository.GetApplicationsInRole(roleId);
            return applications.OrderBy(c => c.Name).ToList();
        }

        public async Task<List<Application>> GetUnassignedApplications()
        {
            var applications = await applicationRepository.GetApplicationsWithNoRoleAssignment();
            return applications.OrderBy(c => c.Name).ToList();
        }

        public async Task<List<Application>> GetUnassignedApplicationsForRole(int id)
        {
            var applications = await applicationRepository.GetApplicationsNotInRole(id);
            return applications.OrderBy(c => c.Name).ToList(); ;
        }

        public async Task AssignApplicationToRole(int applicationId, int roleId)
        {
            await applicationRepository.InsertApplicationRoleMapping(applicationId, roleId);
        }

        public void RemoveApplicationFromRole(int applicationId, int roleId)
        {
            applicationRepository.DeleteApplicationRoleMapping(applicationId, roleId);
        }

        public async Task<List<Role>> GetRoles()
        {
            var roles = await applicationRepository.GetRoles();
            return roles.OrderBy(c => c.Name).ToList(); 
        }

        public async Task AddApplication(string name, string description, string path, bool isExternal)
        {
            var application = new Application()
            {
                Name = name,
                Description = description,
                Path = path,
                IsExternal = isExternal? 1:0
            };
            await applicationRepository.AddApplication(application);
        }

        public async Task<List<Application>> GetAllApplications()
        {
            var applications = await applicationRepository.GetApplications();
            return applications.OrderBy(c => c.Name).ToList();
        }

        public void RemoveApplication(int id)
        {
            applicationRepository.RemoveApplication(id);
        }

    }
}
