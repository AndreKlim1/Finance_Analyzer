using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CaregoryAccountService.Models;

namespace CaregoryAccountService.Repositories
{
    public class CategoryAccountServiceDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public CategoryAccountServiceDbContext(DbContextOptions<CategoryAccountServiceDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;

            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Account> Users { get; set; }
        public DbSet<Category> Profiles { get; set; }
   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryAccountServiceDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));

            base.OnConfiguring(optionsBuilder);
        }
    }
}
