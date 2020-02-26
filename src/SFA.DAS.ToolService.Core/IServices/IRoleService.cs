using SFA.DAS.ToolService.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Core.IServices
{
    public interface IRoleService
    {
        Task<List<ExternalRole>> GetRoles();

        Task SyncIdentityProviderRoles();
    }
}
