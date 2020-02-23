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
    public class Auth0RoleRepository : IAuth0RoleRepository
    {
        private readonly ILogger<Auth0RoleRepository> logger;
        private readonly IApiClient client;
        public Auth0RoleRepository(ILogger<Auth0RoleRepository> _logger, IApiClient _client)
        {
            logger = _logger;
            client = _client;
        }

        public async Task<List<Auth0Role>> GetRoles()
        {
            var roles = await client.GetRoles();
            return roles.Select(c => new Auth0Role { Id = c.Id, Name = c.Name , Description = c.Description}).ToList();
        }
    }
}
