using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BudgetingService.Models;
using Microsoft.EntityFrameworkCore.Design;

namespace BudgetingService.Repositories
{

    public class BudgetingServiceDbContextFactory : IDesignTimeDbContextFactory<BudgetingServiceDbContext>
    {
        public BudgetingServiceDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BudgetingServiceDbContext>();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseNpgsql(connectionString);

            return new BudgetingServiceDbContext(builder.UseNpgsql(connectionString).Options);
        }
    }

    public class BudgetingServiceDbContext : DbContext
    {
        public BudgetingServiceDbContext(DbContextOptions<BudgetingServiceDbContext> options) : base(options)
        {

        }

        public DbSet<Budget> Budgets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("budgets");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BudgetingServiceDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
