using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsersService.Models;

namespace UsersService.Repositories.Configurations
{
    public class ProfileEntityTypeConfigure : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.HasKey(p => p.Id);
            builder.ToTable("Profiles");
            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(32);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(32);

            builder.HasData(
                new Profile
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe"
                },
                new Profile
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Doe"
                }
            );
        }
    }
}
