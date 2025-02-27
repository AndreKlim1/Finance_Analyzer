using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsersService.Models;
using UsersService.Models.Enums;

namespace UsersService.Repositories.Configurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.ToTable("Users");

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(64);
            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(64)
                .IsFixedLength();
            builder.Property(u => u.RegistrationDate)
                .IsRequired();
            builder.Property(u => u.Role)
                .IsRequired()
                .HasConversion<int>();

            builder.HasOne(u => u.Profile)
                .WithOne()
                .HasForeignKey<User>(u => u.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasData(
            new User
            {
                Id = 1,
                Email = "user1@example.com",
                PasswordHash = "h",
                RegistrationDate = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                Role = Role.USER,
                ProfileId = 1
            },
            new User
            {
                Id = 2,
                Email = "user2@example.com",
                PasswordHash = "h",
                RegistrationDate = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                Role = Role.ADMIN,
                ProfileId = 2
            }
        );
        }
    }
}
