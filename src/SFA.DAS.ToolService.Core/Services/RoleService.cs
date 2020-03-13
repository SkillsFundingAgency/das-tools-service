using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Core.Entities;
using SFA.DAS.ToolService.Core.IRepositories;
using SFA.DAS.ToolService.Core.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly ILogger<RoleService> _logger;
        private readonly IExternalRoleRepository _identityProviderRoleRepository;
        private readonly IRoleRepository _localRoleRepository;

        public RoleService(ILogger<RoleService> logger, IExternalRoleRepository identityProviderRoleRepository, IRoleRepository localRoleRepository)
        {
            _logger = logger;
            _identityProviderRoleRepository = identityProviderRoleRepository;
            _localRoleRepository = localRoleRepository;
        }

        public async Task<List<Role>> GetRoles()
        {
            var roles = await _localRoleRepository.GetRoles();
            return roles.OrderBy(c => c.Name).ToList();
        }

        public async Task<Role> GetRole(int roleId)
        {
            return await _localRoleRepository.GetRole(roleId);
        }

        public async Task<List<ExternalRole>> GetExternalRoles()
        {
            return await _identityProviderRoleRepository.GetExternalRoles();
        }

        public async Task SyncIdentityProviderRoles()
        {
            // TODO: try harder
            var idpRoles = await _identityProviderRoleRepository.GetExternalRoles();

            foreach (var role in idpRoles)
            {
                var dbEntity = await _localRoleRepository.GetRole(role.Name);

                if (dbEntity != null)
                {
                    _logger.LogInformation($"Updating role [{dbEntity.Name}] from external idp.");
                    dbEntity.Name = role.Name;
                    dbEntity.ExternalId = role.Id;
                    dbEntity.Description = role.Description;

                    _localRoleRepository.UpdateRole(dbEntity);
                }
                else
                {
                    _logger.LogInformation($"Adding external role [{role.Name}] to database from external idp.");
                    await _localRoleRepository.AddRole(new Role
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
