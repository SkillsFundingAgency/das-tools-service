using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Core.Entities;
using SFA.DAS.ToolService.Core.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IToolServiceDbContext _toolServiceDbContext;

        public RoleRepository(IToolServiceDbContext toolServiceDbContext)
        {
            _toolServiceDbContext = toolServiceDbContext;
        }

        public async Task<List<Role>> GetRoles()
        {
            var result = await _toolServiceDbContext.Role.ToListAsync();

            return result;
        }

        public async Task<Role> GetRole(string name)
        {
            return await _toolServiceDbContext.Role.FirstOrDefaultAsync(role => role.Name.ToLowerInvariant() == name.ToLowerInvariant());
        }

        public async Task<Role> GetRole(int id)
        {
            return await _toolServiceDbContext.Role.SingleOrDefaultAsync(role => role.Id == id);
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
