using SFA.DAS.ToolService.Core.IRepositories;
using SFA.DAS.ToolService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SFA.DAS.ToolService.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IToolServiceDbContext toolServiceDbContext;
        private readonly ILogger<RoleRepository> logger;

        public RoleRepository(IToolServiceDbContext _toolServiceDbContext, ILogger<RoleRepository> _logger)
        {
            toolServiceDbContext = _toolServiceDbContext;
            logger = _logger;
        }

        public async Task<List<Role>> GetRoles()
        {
            var result = await toolServiceDbContext.Role
                .ToListAsync();

            return result;
        }

        public async Task<Role> GetRole(string name)
        {
            return await toolServiceDbContext.Role.FirstOrDefaultAsync(role => role.Name.ToLowerInvariant() == name.ToLowerInvariant());
        }

        public void UpdateRole(Role role)
        { 
            toolServiceDbContext.Role.Update(role);
            toolServiceDbContext.SaveChanges();
        }

        public async Task AddRole(Role role)
        {
            await toolServiceDbContext.Role.AddAsync(role);
            toolServiceDbContext.SaveChanges();
        }
    }
}
