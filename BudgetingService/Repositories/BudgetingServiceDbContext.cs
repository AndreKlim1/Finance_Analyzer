using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BudgetingService.Models;

namespace BudgetingService.Repositories
{
    public class BudgetingServiceDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public BudgetingServiceDbContext(DbContextOptions<BudgetingServiceDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;

            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Budget> Transactions { get; set; }
   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BudgetingServiceDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));

            base.OnConfiguring(optionsBuilder);
        }
    }
}
