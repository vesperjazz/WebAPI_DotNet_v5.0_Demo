using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using WebAPI_DotNetCore_Demo.Domain.Entities.Lookups;

namespace WebAPI_DotNetCore_Demo.Persistence.Configurations
{
    public class GenderConfiguration : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.ToTable(nameof(Gender));

            builder.Property(g => g.Code)
                .HasMaxLength(3)
                .IsRequired();
            builder.HasIndex(g => g.Code)
                .IsUnique();

            builder.Property(g => g.Name)
                .HasMaxLength(10)
                .IsRequired();

            SeedGenders(builder);
        }

        private static void SeedGenders(EntityTypeBuilder<Gender> builder)
        {
            builder.HasData(
                new Gender { ID = new Guid("1af6209b-5cf1-408e-b89b-4bdf8c302c09"), Code = "F", Name = "Female" },
                new Gender { ID = new Guid("a74e7e41-dea4-4c67-add6-785735717cdc"), Code = "M", Name = "Male" },
                new Gender { ID = new Guid("8b6a4475-cda7-4b33-a0f2-8839f755a799"), Code = "U", Name = "Unknown" });
        }
    }
}
