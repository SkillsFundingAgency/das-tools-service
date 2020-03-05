using Microsoft.EntityFrameworkCore;
using SFA.DAS.ToolService.Core.Entities;
using SFA.DAS.ToolService.Core.IRepositories;

namespace SFA.DAS.ToolService.Core
{
    public class ToolServiceDbContext : DbContext, IToolServiceDbContext
    {
        public ToolServiceDbContext(DbContextOptions<ToolServiceDbContext> options) : base(options)
        {
        }

        public DbSet<Application> Application { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<ApplicationRole> ApplicationRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
