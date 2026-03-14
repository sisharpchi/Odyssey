using AuthService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(x => x.FirstName)
            .HasMaxLength(100);

        builder.Property(x => x.LastName)
            .HasMaxLength(100);

        builder.Ignore(x => x.FullName);

        builder.Property(x => x.UserName)
            .HasMaxLength(256);

        builder.Property(x => x.Email)
            .HasMaxLength(256);

        builder.HasIndex(x => x.Email)
            .IsUnique(true);
    }
}
