using SFA.DAS.ToolService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Core.IServices
{
    public interface IRoleService
    {
        Task<List<Auth0Role>> GetRoles();
        Task SyncIdentityProviderRoles();
    }
}
