using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TransactionsService.Models;

namespace TransactionsService.Repositories
{
    public class TransactionsServiceDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public TransactionsServiceDbContext(DbContextOptions<TransactionsServiceDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;

            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Transaction> Transactions { get; set; }
   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransactionsServiceDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));

            base.OnConfiguring(optionsBuilder);
        }
    }
}
