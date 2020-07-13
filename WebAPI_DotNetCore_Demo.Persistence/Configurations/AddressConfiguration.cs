using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;
using WebAPI_DotNetCore_Demo.Domain.Entities;
using WebAPI_DotNetCore_Demo.Domain.Enumerations;

namespace WebAPI_DotNetCore_Demo.Persistence.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable(nameof(Address));

            builder.HasCheckConstraint("CK_Address_AddressType",
                $"AddressType IN ({GetAddressTypeValues()})");

            builder.Property(p => p.AddressType)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(p => p.FirstLine)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.SecondLine)
                .HasMaxLength(100);

            builder.Property(p => p.ThirdLine)
                .HasMaxLength(100);

            builder.Property(p => p.PostCode)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(p => p.City)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.State)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.PersonID)
                .IsRequired();
            builder.HasOne(a => a.Person)
                .WithMany(p => p.Addresses)
                .HasForeignKey(a => a.PersonID)
                .HasConstraintName("FK_Addresses_Persons")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(a => a.CountryID)
                .IsRequired();
            builder.HasOne(a => a.Country)
                .WithMany(c => c.Addresses)
                .HasForeignKey(a => a.CountryID)
                .HasConstraintName("FK_Addresses_Countries")
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static string GetAddressTypeValues()
        {
            return ((AddressType[])Enum.GetValues(typeof(AddressType)))
                .Select(pnt => $"'{pnt}'")
                .Aggregate((first, second) => $"{first}, {second}");
        }
    }
}
