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

            builder.Property(c => c.UserId)
                .IsRequired();

            builder.Property(c => c.CategoryName)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(c => c.CategoryType)
                .IsRequired()
                .HasConversion<int>();

            builder.HasData(
                new Category
                {
                    Id = 1,
                    UserId = 1,
                    CategoryName = "Groceries",
                    CategoryType = CategoryType.EXPENSE
                },
                new Category
                {
                    Id = 2,
                    UserId = 2,
                    CategoryName = "Salary",
                    CategoryType = CategoryType.INCOME
                }
            );
        }
    }
}
