using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionsService.Models;
using TransactionsService.Models.Enums;


namespace TransactionsService.Repositories.Configurations
{
    public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);
            builder.ToTable("Transactions");

            builder.Property(t => t.Value)
                .IsRequired();

            builder.Property(t => t.Currency)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(t => t.CategoryId)
                .IsRequired();

            builder.Property(t => t.AccountId)
                .IsRequired();

            builder.Property(t => t.UserId)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(512);

            builder.Property(t => t.Image)
                .HasMaxLength(256);

            builder.Property(t => t.TransactionDate)
                .IsRequired();

            builder.Property(t => t.CreationDate)
                .IsRequired();

            builder.Property(t => t.PaymentMethod)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(t => t.Merchant)
                .HasMaxLength(128);

            builder.HasData(
                new Transaction
                {
                    Id = 1,
                    Value = 100,
                    Currency = Currency.USD,
                    CategoryId = 1,
                    AccountId = 1,
                    UserId = 1,
                    Description = "Test transaction 1",
                    Image = null,
                    TransactionDate = new DateTime(2025, 2, 1),
                    CreationDate = new DateTime(2025, 2, 1),
                    PaymentMethod = PaymentMethod.CARD,
                    Merchant = "Amazon"
                },
                new Transaction
                {
                    Id = 2,
                    Value = 200,
                    Currency = Currency.EUR,
                    CategoryId = 2,
                    AccountId = 2,
                    UserId = 2,
                    Description = "Test transaction 2",
                    Image = null,
                    TransactionDate = new DateTime(2025, 2, 1),
                    CreationDate = new DateTime(2025, 2, 1),
                    PaymentMethod = PaymentMethod.CASH,
                    Merchant = "Local Store"
                }
            );
        }
    }
}
