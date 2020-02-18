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
        private readonly IApplicationRepository _applicationRepository;

        public ApplicationService(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async Task<List<Application>> GetApplicationsForRole(string[] roles)
        {

            var applications = new List<Application>();
            var publicApplications = await _applicationRepository.GetPublicApplications();
            applications.AddRange(publicApplications);

            foreach (var role in roles)
            {
                var roleId = await _applicationRepository.GetRoleId(role);
                var app = await _applicationRepository.GetApplicationsInRole(roleId);
                applications.AddRange(app);
            }

            return applications;
        }

        public async Task<List<Application>> GetApplicationsForRoleId(int roleId)
        {
            var applications = await _applicationRepository.GetApplicationsInRole(roleId);
            return applications;
        }

        public async Task<List<Application>> GetUnassignedApplicationsForRole(int id)
        {
            var applications = await _applicationRepository.GetApplicationsNotInRole(id);
            return applications;
        }

        public async Task AssignApplicationToRole(int applicationId, int roleId)
        {
            await _applicationRepository.InsertApplicationRoleMapping(applicationId, roleId);
        }

        public void RemoveApplicationFromRole(int applicationId, int roleId)
        {
            _applicationRepository.DeleteApplicationRoleMapping(applicationId, roleId);
        }

        public async Task<List<Role>> GetRoles()
        {
            return await _applicationRepository.GetRoles();
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
            await _applicationRepository.AddApplication(application);
        }
    }
}
