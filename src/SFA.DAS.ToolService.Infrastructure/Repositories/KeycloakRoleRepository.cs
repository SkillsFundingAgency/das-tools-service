using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Core.Entities;
using SFA.DAS.ToolService.Core.IRepositories;
using SFA.DAS.ToolService.Infrastructure.Keycloak;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Infrastructure.Repositories
{
    public class KeycloakRoleRepository : IExternalRoleRepository
    {
        private readonly ILogger<KeycloakRoleRepository> _logger;
        private readonly IKeycloakApiClient _client;

        public KeycloakRoleRepository(ILogger<KeycloakRoleRepository> logger, IKeycloakApiClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<List<ExternalRole>> GetExternalRoles()
        {
            var roles = await _client.GetKeycloakRoles();
            return roles.Select(c => new ExternalRole { Id = c.Id, Name = c.Name, Description = c.Description }).ToList();
        }
    }
}
