using Microsoft.EntityFrameworkCore;
using SFA.DAS.ToolService.Core.Entities;

namespace SFA.DAS.ToolService.Core.IRepositories
{
    public interface IToolServiceDbContext
    {
        DbSet<Application> Application { get; set; }
        DbSet<Role> Role { get; set; }
        DbSet<ApplicationRole> ApplicationRole { get; set; }

        int SaveChanges();
    }
}
