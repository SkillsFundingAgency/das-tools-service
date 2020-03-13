using SFA.DAS.ToolService.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Core.IServices
{
    public interface IRoleService
    {
        Task<List<Role>> GetRoles();

        Task<Role> GetRole(int id);

        Task<List<ExternalRole>> GetExternalRoles();

        Task SyncIdentityProviderRoles();
    }
}
