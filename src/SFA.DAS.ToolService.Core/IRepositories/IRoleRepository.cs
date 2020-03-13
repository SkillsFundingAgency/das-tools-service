using SFA.DAS.ToolService.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Core.IRepositories
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetRoles();

        Task<Role> GetRole(string name);

        Task<Role> GetRole(int id);

        void UpdateRole(Role role);

        Task AddRole(Role role);
    }
}
