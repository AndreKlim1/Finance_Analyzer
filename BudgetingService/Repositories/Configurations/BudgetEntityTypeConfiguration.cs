using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BudgetingService.Models;
using BudgetingService.Models.Enums;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore.ChangeTracking;


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

            builder.Property(b => b.AccountIds)
            .HasConversion(
            v => v == null ? null : v.ToArray(),
            v => v == null ? null : v.ToList(),
            new ValueComparer<List<long>>(
                (c1, c2) => c1 == null && c2 == null || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
                c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c == null ? null : c.ToList()) 

            );

            builder.Property(b => b.CategoryIds)
            .HasConversion(
            v => v == null ? null : v.ToArray(),
            v => v == null ? null : v.ToList(),
            new ValueComparer<List<long>>(
                (c1, c2) => c1 == null && c2 == null || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
                c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c == null ? null : c.ToList())
            );

            builder.Property(b => b.BudgetName)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(b => b.Description)
                .HasMaxLength(512);

            builder.Property(b => b.PlannedAmount)
                .IsRequired();

            builder.Property(b => b.CurrValue)
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

            builder.Property(b => b.WarningShowed)
                .IsRequired();

            builder.Property(b => b.Color)
                .HasMaxLength(32)
                .IsRequired();

            builder.HasData(
                new Budget
                {
                    Id = 1,
                    UserId = 1,
                    CategoryIds = new List<long> { 4, 6 },
                    AccountIds = new List<long> { 1 },
                    BudgetName = "Monthly Groceries",
                    Description = "Budget for monthly grocery shopping",
                    PlannedAmount = 500,
                    CurrValue = 100,
                    Currency = Currency.USD,
                    PeriodStart = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    PeriodEnd = new DateTime(2025, 3, 31, 0, 0, 0, DateTimeKind.Utc),
                    BudgetStatus = BudgetStatus.ACTIVE,
                    BudgetType = BudgetType.EXPENSES,
                    WarningThreshold = 80,
                    WarningShowed = false,
                    Color = "#3f3dbf"
                },
                new Budget
                {
                    Id = 2,
                    UserId = 2,
                    CategoryIds = new List<long> { 3, 5 },
                    AccountIds = new List<long> { 2, 4 },
                    BudgetName = "Vacation Savings",
                    Description = "Saving up for a summer vacation",
                    PlannedAmount = 2000,
                    CurrValue = 200,
                    Currency = Currency.EUR,
                    PeriodStart = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    PeriodEnd = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                    BudgetStatus = BudgetStatus.ACTIVE,
                    BudgetType = BudgetType.SAVINGS,
                    WarningThreshold = 90,
                    WarningShowed = false,
                    Color = "#34bbb7"
                }
            );
        }
    }
}
