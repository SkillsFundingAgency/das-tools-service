using SFA.DAS.ToolService.Core.Entities;
using SFA.DAS.ToolService.Core.IServices;
using SFA.DAS.ToolService.Core.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace SFA.DAS.ToolService.Core.Services
{
    public class RoleService: IRoleService
    {
        private readonly ILogger<RoleService> logger;
        private readonly IAuth0RoleRepository identityProviderRoleRepository;
        private readonly IRoleRepository localRoleRepository;

        public RoleService(ILogger<RoleService> _logger, IAuth0RoleRepository _identityProviderRoleRepository, IRoleRepository _localRoleRepository)
        {
            logger = _logger;
            identityProviderRoleRepository = _identityProviderRoleRepository;
            localRoleRepository = _localRoleRepository;
        }


        public async Task<List<Auth0Role>> GetRoles()
        {
            return await identityProviderRoleRepository.GetRoles();
        }

        public async Task SyncIdentityProviderRoles()
        {
            // TODO: try harder
            var idpRoles = await identityProviderRoleRepository.GetRoles();

            foreach (var role in idpRoles)
            {
                var dbEntity = await localRoleRepository.GetRole(role.Name);

                if (dbEntity != null)
                {
                    logger.LogInformation($"Updating role [{dbEntity.Name}] from external idp.");
                    dbEntity.Name = role.Name;
                    dbEntity.ExternalId = role.Id;
                    dbEntity.Description = role.Description;

                    localRoleRepository.UpdateRole(dbEntity);
                } else
                {
                    logger.LogInformation($"Adding external role [{role.Name}] to database from external idp.");
                    await localRoleRepository.AddRole(new Role
                    {
                        Name = role.Name,
                        ExternalId = role.Id,
                        Description = role.Description
                    });
                }
            }
        }
    }
}
