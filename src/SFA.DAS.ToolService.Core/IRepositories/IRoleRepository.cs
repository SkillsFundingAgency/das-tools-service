using SFA.DAS.ToolService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Core.IRepositories
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetRoles();
        Task<Role> GetRole(string name);
        void UpdateRole(Role role);
        Task AddRole(Role role);

    }
}
