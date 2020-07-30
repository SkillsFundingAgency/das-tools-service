using SFA.DAS.ToolService.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Infrastructure.Keycloak
{
    public interface IKeycloakApiClient
    {
        Task<IList<ExternalRole>> GetKeycloakRoles();
    }
}
