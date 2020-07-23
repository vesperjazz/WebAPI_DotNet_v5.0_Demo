using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using WebAPI_DotNetCore_Demo.Domain.Entities.Users;

namespace WebAPI_DotNetCore_Demo.Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(nameof(Role));

            builder.Property(r => r.Name)
                .HasMaxLength(30)
                .IsRequired();

            SeedRoles(builder);
        }

        private static void SeedRoles(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role { ID = new Guid("dab0807c-822c-4258-ad79-07dd543cb253"), Name = "Admin" },
                new Role { ID = new Guid("99626019-0a7d-4c79-9058-b727ed7b1fa9"), Name = "User" });
        }
    }
}
