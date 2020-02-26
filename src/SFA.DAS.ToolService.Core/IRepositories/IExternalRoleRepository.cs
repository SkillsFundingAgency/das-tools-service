using SFA.DAS.ToolService.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Core.IRepositories
{
    public interface IExternalRoleRepository
    {
        Task<List<ExternalRole>> GetExternalRoles();
    }
}
