using SFA.DAS.ToolService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Core.IServices
{
    public interface IApplicationService
    {
        Task<List<Application>> GetAllApplications();
        Task<List<Application>> GetApplicationsForRole(string[] id);
        Task<List<Application>> GetApplicationsForRoleId(int id);
        Task<List<Application>> GetUnassignedApplications();
        Task<List<Application>> GetUnassignedApplicationsForRole(int id);
        Task<List<Role>> GetRoles();
        Task AssignApplicationToRole(int applicationId, int roleId);
        Task AddApplication(string name, string description, string path, bool isExternal);
        void RemoveApplicationFromRole(int applicationId, int roleId);
        void RemoveApplication(int id);
    }
}
