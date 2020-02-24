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
        private readonly IToolServiceDbContext _toolServiceDbContext;
        private readonly ILogger<RoleRepository> _logger;

        public RoleRepository(IToolServiceDbContext toolServiceDbContext, ILogger<RoleRepository> logger)
        {
            _toolServiceDbContext = toolServiceDbContext;
            _logger = logger;
        }

        public async Task<List<Role>> GetRoles()
        {
            var result = await _toolServiceDbContext.Role
                .ToListAsync();

            return result;
        }

        public async Task<Role> GetRole(string name)
        {
            return await _toolServiceDbContext.Role.FirstOrDefaultAsync(role => role.Name.ToLowerInvariant() == name.ToLowerInvariant());
        }

        public void UpdateRole(Role role)
        { 
            _toolServiceDbContext.Role.Update(role);
            _toolServiceDbContext.SaveChanges();
        }

        public async Task AddRole(Role role)
        {
            await _toolServiceDbContext.Role.AddAsync(role);
            _toolServiceDbContext.SaveChanges();
        }
    }
}
