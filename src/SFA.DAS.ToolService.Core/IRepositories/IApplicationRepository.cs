using SFA.DAS.ToolService.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Core.IRepositories
{
    public interface IApplicationRepository
    {
        Task<List<Application>> GetApplicationsByRoleName(string role);

        Task<List<Application>> GetApplicationsInRole(int id);

        Task<List<Application>> GetApplicationsNotInRole(int id);

        Task<List<Application>> GetPublicApplications();

        Task<List<Application>> GetApplications();

        Task<List<Application>> GetApplicationsWithNoRoleAssignment();

        Task<List<Role>> GetRoles();

        Task<int> GetRoleId(string name);

        Task InsertApplicationRoleMapping(int applicationId, int roleId);

        void DeleteApplicationRoleMapping(int applicationId, int roleId);

        Task AddApplication(Application application);

        void RemoveApplication(int id);
    }
}
