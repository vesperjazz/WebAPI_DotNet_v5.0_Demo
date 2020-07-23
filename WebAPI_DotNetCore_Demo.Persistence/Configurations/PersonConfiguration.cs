using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI_DotNetCore_Demo.Domain.Entities;
using WebAPI_DotNetCore_Demo.Persistence.Extensions;

namespace WebAPI_DotNetCore_Demo.Persistence.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable(nameof(Person));

            builder.Property(p => p.FirstName)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(p => p.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(p => p.Gender)
                .WithMany(g => g.Persons)
                .HasForeignKey(p => p.GenderID);

            builder.BuildUserAuditColumns();
        }
    }
}
