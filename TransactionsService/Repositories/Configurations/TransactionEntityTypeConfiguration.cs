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

            builder.Property(t => t.Title)
                .HasMaxLength(128);

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

            builder.Property(t => t.TransactionType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(t => t.Merchant)
                .HasMaxLength(128);

            builder.HasData(
                new Transaction
                {
                    Id = 1,
                    Value = 400,
                    Title = "First transaction",
                    Currency = Currency.USD,
                    CategoryId = 3,
                    AccountId = 1,
                    UserId = 1,
                    Description = "Test transaction 1",
                    Image = null,
                    TransactionDate = new DateTime(2025, 4, 7, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 4, 7, 0, 0, 0, DateTimeKind.Utc),
                    TransactionType = TransactionType.INCOME,
                    Merchant = "Amazon"
                },
                new Transaction
                {
                    Id = 2,
                    Value = -200,
                    Title = "Something for home",
                    Currency = Currency.EUR,
                    CategoryId = 4,
                    AccountId = 2,
                    UserId = 1,
                    Description = "Test transaction 2",
                    Image = null,
                    TransactionDate = new DateTime(2025, 4, 8, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 4, 8, 0, 0, 0, DateTimeKind.Utc),
                    TransactionType = TransactionType.EXPENSE,
                    Merchant = "Local Store"
                },
                new Transaction
                {
                    Id = 3,
                    Value = -400,
                    Title = "Cheeeeese",
                    Currency = Currency.EUR,
                    CategoryId = 6,
                    AccountId = 2,
                    UserId = 1,
                    Description = "Test transaction 2",
                    Image = null,
                    TransactionDate = new DateTime(2025, 4, 10, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 4, 10, 0, 0, 0, DateTimeKind.Utc),
                    TransactionType = TransactionType.EXPENSE,
                    Merchant = "Local Store"
                },
                new Transaction
                {
                    Id = 4,
                    Value = 100,
                    Title = "Coins from fountain",
                    Currency = Currency.USD,
                    CategoryId = 5,
                    AccountId = 2,
                    UserId = 1,
                    Description = "Test transaction 2",
                    Image = null,
                    TransactionDate = new DateTime(2025, 4, 12, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 4, 12, 0, 0, 0, DateTimeKind.Utc),
                    TransactionType = TransactionType.INCOME,
                    Merchant = "Local Store"
                },
                new Transaction
                {
                    Id = 5,
                    Value = 100,
                    Title = "Another USER transaction",
                    Currency = Currency.USD,
                    CategoryId = 3,
                    AccountId = 7,
                    UserId = 2,
                    Description = "Test transaction 1",
                    Image = null,
                    TransactionDate = new DateTime(2025, 4, 7, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 4, 7, 0, 0, 0, DateTimeKind.Utc),
                    TransactionType = TransactionType.INCOME,
                    Merchant = "Amazon"
                },
                new Transaction
                {
                    Id = 6,
                    Value = 1100,
                    Title = "Finally Salary",
                    Currency = Currency.USD,
                    CategoryId = 3,
                    AccountId = 1,
                    UserId = 1,
                    Description = "Test transaction 1",
                    Image = null,
                    TransactionDate = new DateTime(2025, 4, 14, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 4, 14, 0, 0, 0, DateTimeKind.Utc),
                    TransactionType = TransactionType.INCOME,
                    Merchant = "Amazon"
                },
                new Transaction
                {
                    Id = 7,
                    Value = -200,
                    Title = "New Tshirt",
                    Currency = Currency.USD,
                    CategoryId = 9,
                    AccountId = 4,
                    UserId = 1,
                    Description = "Test transaction 1",
                    Image = null,
                    TransactionDate = new DateTime(2025, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                    TransactionType = TransactionType.EXPENSE,
                    Merchant = "Amazon"
                },
                new Transaction
                {
                    Id = 8,
                    Value = -400,
                    Title = "Movie tickets",
                    Currency = Currency.USD,
                    CategoryId = 10,
                    AccountId = 3,
                    UserId = 1,
                    Description = "Test transaction 1",
                    Image = null,
                    TransactionDate = new DateTime(2025, 4, 28, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 4, 28, 0, 0, 0, DateTimeKind.Utc),
                    TransactionType = TransactionType.EXPENSE,
                    Merchant = "Amazon"
                },
                new Transaction
                {
                    Id = 9,
                    Value = 600,
                    Title = "Parents help",
                    Currency = Currency.EUR,
                    CategoryId = 8,
                    AccountId = 6,
                    UserId = 1,
                    Description = "Test transaction 1",
                    Image = null,
                    TransactionDate = new DateTime(2025, 4, 20, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 4, 20, 0, 0, 0, DateTimeKind.Utc),
                    TransactionType = TransactionType.INCOME,
                    Merchant = "Amazon"
                },
                new Transaction
                {
                    Id = 10,
                    Value = -100,
                    Title = "Food from market",
                    Currency = Currency.USD,
                    CategoryId = 11,
                    AccountId = 2,
                    UserId = 1,
                    Description = "Test transaction 1",
                    Image = null,
                    TransactionDate = new DateTime(2025, 4, 25, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 4, 25, 0, 0, 0, DateTimeKind.Utc),
                    TransactionType = TransactionType.EXPENSE,
                    Merchant = "Amazon"
                },
                new Transaction
                {
                    Id = 11,
                    Value = -200,
                    Title = "Cafe breakfast",
                    Currency = Currency.USD,
                    CategoryId = 6,
                    AccountId = 1,
                    UserId = 1,
                    Description = "Test transaction 1",
                    Image = null,
                    TransactionDate = new DateTime(2025, 4, 25, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 4, 25, 0, 0, 0, DateTimeKind.Utc),
                    TransactionType = TransactionType.EXPENSE,
                    Merchant = "Amazon"
                },
                new Transaction
                {
                    Id = 12,
                    Value = 200,
                    Title = "Some money for helping",
                    Currency = Currency.USD,
                    CategoryId = 5,
                    AccountId = 5,
                    UserId = 1,
                    Description = "Test transaction 1",
                    Image = null,
                    TransactionDate = new DateTime(2025, 4, 4, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 4, 4, 0, 0, 0, DateTimeKind.Utc),
                    TransactionType = TransactionType.INCOME,
                    Merchant = "Amazon"
                },
                new Transaction
                {
                    Id = 13,
                    Value = -300,
                    Title = "Some new dishes",
                    Currency = Currency.USD,
                    CategoryId = 4,
                    AccountId = 1,
                    UserId = 1,
                    Description = "Test transaction 1",
                    Image = null,
                    TransactionDate = new DateTime(2025, 4, 26, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 4, 26, 0, 0, 0, DateTimeKind.Utc),
                    TransactionType = TransactionType.EXPENSE,
                    Merchant = "Amazon"
                },
                new Transaction
                {
                    Id = 14,
                    Value = 1100,
                    Title = "Salary again",
                    Currency = Currency.USD,
                    CategoryId = 3,
                    AccountId = 4,
                    UserId = 1,
                    Description = "Test transaction 1",
                    Image = null,
                    TransactionDate = new DateTime(2025, 4, 28, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 4, 28, 0, 0, 0, DateTimeKind.Utc),
                    TransactionType = TransactionType.INCOME,
                    Merchant = "Amazon"
                },
                new Transaction
                {
                    Id = 15,
                    Value = -1000,
                    Title = "Flat rent",
                    Currency = Currency.USD,
                    CategoryId = 12,
                    AccountId = 4,
                    UserId = 1,
                    Description = "Test transaction 1",
                    Image = null,
                    TransactionDate = new DateTime(2025, 4, 29, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 4, 29, 0, 0, 0, DateTimeKind.Utc),
                    TransactionType = TransactionType.EXPENSE,
                    Merchant = "Amazon"
                },
                new Transaction
                {
                    Id = 16,
                    Value = -300,
                    Title = "Another day, another cafe",
                    Currency = Currency.USD,
                    CategoryId = 6,
                    AccountId = 3,
                    UserId = 1,
                    Description = "Test transaction 1",
                    Image = null,
                    TransactionDate = new DateTime(2025, 4, 29, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 4, 29, 0, 0, 0, DateTimeKind.Utc),
                    TransactionType = TransactionType.EXPENSE,
                    Merchant = "Amazon"
                }
            );
        }
    }
}
