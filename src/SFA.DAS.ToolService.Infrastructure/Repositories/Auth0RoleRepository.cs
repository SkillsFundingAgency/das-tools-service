using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Core.Entities;
using SFA.DAS.ToolService.Core.IRepositories;
using SFA.DAS.ToolService.Infrastructure.Auth0;

namespace SFA.DAS.ToolService.Infrastructure.Repositories
{
    public class Auth0RoleRepository : IExternalRoleRepository
    {
        private readonly ILogger<Auth0RoleRepository> _logger;
        private readonly IAuth0ApiClient _client;
        public Auth0RoleRepository(ILogger<Auth0RoleRepository> logger, IAuth0ApiClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<List<ExternalRole>> GetExternalRoles()
        {
            var roles = await _client.GetAuth0Roles();
            return roles.Select(c => new ExternalRole { Id = c.Id, Name = c.Name, Description = c.Description }).ToList();
        }
    }
}
