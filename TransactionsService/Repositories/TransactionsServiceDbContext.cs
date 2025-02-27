using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TransactionsService.Models;

namespace TransactionsService.Repositories
{

    public class TransactionsServiceDbContextFactory : IDesignTimeDbContextFactory<TransactionsServiceDbContext>
    {
        public TransactionsServiceDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TransactionsServiceDbContext>();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseNpgsql(connectionString);

            return new TransactionsServiceDbContext(builder.UseNpgsql(connectionString).Options);
        }
    }

    public class TransactionsServiceDbContext : DbContext
    {

        public TransactionsServiceDbContext(DbContextOptions<TransactionsServiceDbContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("transactions");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransactionsServiceDbContext).Assembly);
        }

        
    }
}
