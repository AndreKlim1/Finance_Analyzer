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

            builder.Property(c => c.Color)
                .IsRequired();

            builder.HasData(
                new Category
                {
                    Id = 1,
                    UserId = null,
                    CategoryName = "Transfer",
                    CategoryType = CategoryType.TRANSFER,
                    Icon = "/assets/exchange-arrows.png",
                    Color = "#6572bd"
                },
                new Category
                {
                    Id = 2,
                    UserId = null,
                    CategoryName = "Correction",
                    CategoryType = CategoryType.CORRECTION,
                    Icon = "/assets/categories/settings.png",
                    Color = "#6572bd"
                },
                new Category
                {
                    Id = 3,
                    UserId = null,
                    CategoryName = "Salary",
                    CategoryType = CategoryType.INCOME,
                    Icon = "/assets/categories/bills.png",
                    Color = "#6572bd"
                },
                new Category
                {
                    Id = 4,
                    UserId = null,
                    CategoryName = "Shopping",
                    CategoryType = CategoryType.EXPENSE,
                    Icon = "/assets/categories/shopping.png",
                    Color = "#6572bd"
                },
                new Category
                {
                    Id = 5,
                    UserId = null,
                    CategoryName = "Random money",
                    CategoryType = CategoryType.INCOME,
                    Icon = "/assets/categories/wallet.png",
                    Color = "#6572bd"
                },
                new Category
                {
                    Id = 6,
                    UserId = null,
                    CategoryName = "Restaurants",
                    CategoryType = CategoryType.EXPENSE,
                    Icon = "/assets/categories/curry.png",
                    Color = "#6572bd"
                },
                new Category
                {
                    Id = 7,
                    UserId = null,
                    CategoryName = "Passive gain",
                    CategoryType = CategoryType.INCOME,
                    Icon = "/assets/increase.png",
                    Color = "#6572bd"
                },
                new Category
                {
                    Id = 8,
                    UserId = null,
                    CategoryName = "Other sources",
                    CategoryType = CategoryType.INCOME,
                    Icon = "/assets/categories/code.png",
                    Color = "#6572bd"
                },
                new Category
                {
                    Id = 9,
                    UserId = null,
                    CategoryName = "Clothes",
                    CategoryType = CategoryType.EXPENSE,
                    Icon = "/assets/categories/tshirt.png",
                    Color = "#6572bd"
                },
                new Category
                {
                    Id = 10,
                    UserId = null,
                    CategoryName = "Entertainment",
                    CategoryType = CategoryType.EXPENSE,
                    Icon = "/assets/categories/popcorn.png",
                    Color = "#6572bd"
                },
                new Category
                {
                    Id = 11,
                    UserId = null,
                    CategoryName = "Foodstuff",
                    CategoryType = CategoryType.EXPENSE,
                    Icon = "/assets/categories/shopping-cart.png",
                    Color = "#6572bd"
                },
                new Category
                {
                    Id = 12,
                    UserId = null,
                    CategoryName = "Home expenses",
                    CategoryType = CategoryType.EXPENSE,
                    Icon = "/assets/categories/house.png",
                    Color = "#6572bd"
                }
            );
        }
    }
}
