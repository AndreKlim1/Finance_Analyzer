using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaregoryAccountService.Models;
using CaregoryAccountService.Models.Enums;

namespace CaregoryAccountService.Repositories.Configurations
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
            builder.ToTable("Categories");

            builder.Property(c => c.UserId);

            builder.Property(c => c.CategoryName)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(c => c.CategoryType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(c => c.Icon)
                .IsRequired();

            builder.HasData(
                new Category
                {
                    Id = 1,
                    UserId = null,
                    CategoryName = "Transfer",
                    CategoryType = CategoryType.TRANSFER,
                    Icon = "/assets/exchange-arrows.png"
                },
                new Category
                {
                    Id = 2,
                    UserId = null,
                    CategoryName = "Correction",
                    CategoryType = CategoryType.CORRECTION,
                    Icon = "/assets/increase.png"
                },
                new Category
                {
                    Id = 3,
                    UserId = null,
                    CategoryName = "Salary",
                    CategoryType = CategoryType.INCOME,
                    Icon = "/assets/categories/bills.png"
                },
                new Category
                {
                    Id = 4,
                    UserId = null,
                    CategoryName = "Shopping",
                    CategoryType = CategoryType.EXPENSE,
                    Icon = "/assets/categories/shopping.png"
                },
                new Category
                {
                    Id = 5,
                    UserId = null,
                    CategoryName = "Random money",
                    CategoryType = CategoryType.INCOME,
                    Icon = "/assets/categories/wallet.png"
                },
                new Category
                {
                    Id = 6,
                    UserId = null,
                    CategoryName = "Food",
                    CategoryType = CategoryType.EXPENSE,
                    Icon = "/assets/categories/curry.png"
                }
            );
        }
    }
}
