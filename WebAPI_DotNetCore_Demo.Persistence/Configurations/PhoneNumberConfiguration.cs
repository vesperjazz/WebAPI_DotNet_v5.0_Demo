using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;
using WebAPI_DotNetCore_Demo.Domain.Entities;
using WebAPI_DotNetCore_Demo.Domain.Enumerations;

namespace WebAPI_DotNetCore_Demo.Persistence.Configurations
{
    public class PhoneNumberConfiguration : IEntityTypeConfiguration<PhoneNumber>
    {
        public void Configure(EntityTypeBuilder<PhoneNumber> builder)
        {
            builder.ToTable(nameof(PhoneNumber));

            builder.HasCheckConstraint("CK_PhoneNumber_PhoneNumberType",
                $"PhoneNumberType IN ({GetPhoneNumberTypeValues()})");

            builder.Property(pn => pn.PhoneNumberType)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(pn => pn.Number)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasOne(pn => pn.Country)
                .WithMany(c => c.PhoneNumbers)
                .HasForeignKey(pn => pn.CountryID)
                .HasConstraintName("FK_PhoneNumbers_Countries")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(pn => pn.PersonID)
                .IsRequired();
            builder.HasOne(pn => pn.Person)
                .WithMany(p => p.PhoneNumbers)
                .HasForeignKey(pn => pn.PersonID)
                .HasConstraintName("FK_PhoneNumbers_Persons")
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static string GetPhoneNumberTypeValues()
        {
            return ((PhoneNumberType[])Enum.GetValues(typeof(PhoneNumberType)))
                .Select(pnt => $"'{pnt}'")
                .Aggregate((first, second) => $"{first}, {second}");
        }
    }
}
