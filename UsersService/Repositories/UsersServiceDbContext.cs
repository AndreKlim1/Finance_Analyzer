using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UsersService.Models;

namespace UsersService.Repositories
{
    public class UsersServiceDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public UsersServiceDbContext(DbContextOptions<UsersServiceDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;

            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsersServiceDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));

            base.OnConfiguring(optionsBuilder);
        }
    }
}
