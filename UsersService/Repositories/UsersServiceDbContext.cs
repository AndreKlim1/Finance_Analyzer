using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using UsersService.Models;

namespace UsersService.Repositories
{

    public class UsersServiceDbContextFactory : IDesignTimeDbContextFactory<UsersServiceDbContext>
    {
        public UsersServiceDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<UsersServiceDbContext>();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseNpgsql(connectionString);

            return new UsersServiceDbContext(builder.UseNpgsql(connectionString).Options);
        }
    }

    public class UsersServiceDbContext : DbContext
    {
        public UsersServiceDbContext(DbContextOptions<UsersServiceDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("users");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsersServiceDbContext).Assembly);
        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));

            base.OnConfiguring(optionsBuilder);
        }*/
    }
}
