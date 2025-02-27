using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BudgetingService.Models;
using BudgetingService.Models.Enums;


namespace BudgetingService.Repositories.Configurations
{
    public class BudgetEntityTypeConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.HasKey(b => b.Id);
            builder.ToTable("Budgets");

            builder.Property(b => b.UserId)
                .IsRequired();

            builder.Property(b => b.CategoryId);

            builder.Property(b => b.BudgetName)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(b => b.Description)
                .HasMaxLength(512);

            builder.Property(b => b.PlannedAmount)
                .IsRequired();

            builder.Property(b => b.Currency)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(b => b.PeriodStart)
                .IsRequired();

            builder.Property(b => b.PeriodEnd)
                .IsRequired();

            builder.Property(b => b.BudgetStatus)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(b => b.BudgetType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(b => b.WarningThreshold)
                .IsRequired();

            builder.HasData(
                new Budget
                {
                    Id = 1,
                    UserId = 1,
                    CategoryId = 1,
                    BudgetName = "Monthly Groceries",
                    Description = "Budget for monthly grocery shopping",
                    PlannedAmount = 500,
                    Currency = Currency.USD,
                    PeriodStart = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    PeriodEnd = new DateTime(2025, 3, 31, 0, 0, 0, DateTimeKind.Utc),
                    BudgetStatus = BudgetStatus.ACTIVE,
                    BudgetType = BudgetType.EXPENSE,
                    WarningThreshold = 80
                },
                new Budget
                {
                    Id = 2,
                    UserId = 2,
                    CategoryId = 2,
                    BudgetName = "Vacation Savings",
                    Description = "Saving up for a summer vacation",
                    PlannedAmount = 2000,
                    Currency = Currency.EUR,
                    PeriodStart = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    PeriodEnd = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                    BudgetStatus = BudgetStatus.FINISHED,
                    BudgetType = BudgetType.SAVINGS,
                    WarningThreshold = 90
                }
            );
        }
    }
}
