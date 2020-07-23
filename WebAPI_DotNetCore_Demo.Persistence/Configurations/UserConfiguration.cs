using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI_DotNetCore_Demo.Domain.Entities.Users;
using WebAPI_DotNetCore_Demo.Persistence.Extensions;

namespace WebAPI_DotNetCore_Demo.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));

            builder.HasAlternateKey(u => u.UserName);

            builder.Property(u => u.UserName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.PasswordHash)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(u => u.PasswordSalt)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(u => u.IsActive)
                .IsRequired();

            builder.BuildUserAuditColumns();
        }
    }
}
