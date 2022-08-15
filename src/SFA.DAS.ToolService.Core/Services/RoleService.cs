using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Core.Entities;
using SFA.DAS.ToolService.Core.IRepositories;
using SFA.DAS.ToolService.Core.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _localRoleRepository;

        public RoleService(IRoleRepository localRoleRepository)
        {
            _localRoleRepository = localRoleRepository;
        }

        public async Task<List<Role>> GetRoles()
        {
            var roles = await _localRoleRepository.GetRoles();
            return roles.OrderBy(c => c.Name).ToList();
        }

        public async Task<Role> GetRole(int roleId)
        {
            return await _localRoleRepository.GetRole(roleId);
        }

    }
}
