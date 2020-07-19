using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI_DotNetCore_Demo.Domain.Entities;

namespace WebAPI_DotNetCore_Demo.Persistence.Configurations
{
    public class APILogConfiguration : IEntityTypeConfiguration<APILog>
    {
        public void Configure(EntityTypeBuilder<APILog> builder)
        {
            builder.ToTable(nameof(APILog));

            builder.Property(al => al.Level)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(al => al.RequestMethod)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(al => al.UserName)
                .HasMaxLength(100);

            builder.Property(al => al.Environment)
                .HasMaxLength(20);
        }
    }
}
