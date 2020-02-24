using System;
using System.Collections.Generic;
using SFA.DAS.ToolService.Core.Entities;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Core.IRepositories
{
    public interface IExternalRoleRepository
    {
        Task<List<ExternalRole>> GetExternalRoles();
    }
}
