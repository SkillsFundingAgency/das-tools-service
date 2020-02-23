using System;
using System.Collections.Generic;
using SFA.DAS.ToolService.Core.Entities;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Core.IRepositories
{
    public interface IAuth0RoleRepository
    {
        Task<List<Auth0Role>> GetRoles();
    }
}
