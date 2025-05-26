using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaregoryAccountService.Models;
using CaregoryAccountService.Models.Enums;

namespace CaregoryAccountService.Repositories.Configurations
{
    public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.Id);
            builder.ToTable("Accounts");

            builder.Property(a => a.UserId)
                .IsRequired();

            builder.Property(a => a.AccountName)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(a => a.AccountType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(a => a.Currency)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(a => a.Balance)
                .IsRequired();

            builder.Property(a => a.TransactionsCount)
                .IsRequired();

            builder.Property(a => a.Description)
                .HasMaxLength(512);

            builder.Property(a => a.Color)
                .HasMaxLength(32);

            builder.HasData(
                new Account
                {
                    Id = 1,
                    UserId = 1,
                    AccountName = "Bank",
                    AccountType = AccountType.CREDIT_CARD,
                    Currency = Currency.USD,
                    Balance = 1500,
                    TransactionsCount = 0,
                    Description = "Primary checking account",
                    Color = "#be6464"
                },
                new Account
                {
                    Id = 2,
                    UserId = 1,
                    AccountName = "Cash",
                    AccountType = AccountType.CASH,
                    Currency = Currency.EUR,
                    Balance = 5000,
                    TransactionsCount = 0,
                    Description = "Long-term savings account",
                    Color = "#187547"
                },
                new Account
                {
                    Id = 3,
                    UserId = 1,
                    AccountName = "Second Cash",
                    AccountType = AccountType.CASH,
                    Currency = Currency.EUR,
                    Balance = 3000,
                    TransactionsCount = 0,
                    Description = "Long-term savings account",
                    Color = "#6572bd"
                },
                new Account
                {
                    Id = 4,
                    UserId = 1,
                    AccountName = "Another bank",
                    AccountType = AccountType.CREDIT_CARD,
                    Currency = Currency.EUR,
                    Balance = 3000,
                    TransactionsCount = 0,
                    Description = "Long-term savings account",
                    Color = "#7d4aad"
                },
                new Account
                {
                    Id = 5,
                    UserId = 1,
                    AccountName = "For black day",
                    AccountType = AccountType.CASH,
                    Currency = Currency.USD,
                    Balance = 400,
                    TransactionsCount = 0,
                    Description = "Long-term savings account",
                    Color = "#ccc670"
                },
                new Account
                {
                    Id = 6,
                    UserId = 1,
                    AccountName = "Free money",
                    AccountType = AccountType.CREDIT_CARD,
                    Currency = Currency.EUR,
                    Balance = 500,
                    TransactionsCount = 0,
                    Description = "Long-term savings account",
                    Color = "#80bed6"
                }


            );
        }
    }

}
