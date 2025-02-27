using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CaregoryAccountService.Models;
using Microsoft.EntityFrameworkCore.Design;

namespace CaregoryAccountService.Repositories
{

    public class CategoryAccountsServiceDbContextFactory : IDesignTimeDbContextFactory<CategoryAccountServiceDbContext>
    {
        public CategoryAccountServiceDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CategoryAccountServiceDbContext>();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseNpgsql(connectionString);

            return new CategoryAccountServiceDbContext(builder.UseNpgsql(connectionString).Options);
        }
    }

    public class CategoryAccountServiceDbContext : DbContext
    {

        public CategoryAccountServiceDbContext(DbContextOptions<CategoryAccountServiceDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("category_accounts");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryAccountServiceDbContext).Assembly);
        }

    }
}
