using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI_DotNetCore_Demo.Domain.Entities.Bases;

namespace WebAPI_DotNetCore_Demo.Persistence.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
        public static void BuildUserAuditColumns<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : AuditEntityBase
        {
            builder.Property(u => u.CreateDate)
                .IsRequired();

            builder.Property(u => u.CreateByUserID)
                .IsRequired();

            builder.HasOne(u => u.CreateByUser)
                .WithMany($"CreateBy{typeof(TEntity).Name}s")
                .HasForeignKey(u => u.CreateByUserID)
                .HasConstraintName($"FK_{typeof(TEntity).Name}s_CreateBy{typeof(TEntity).Name}s")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(u => u.UpdateDate)
                .IsRequired();

            builder.Property(u => u.UpdateByUserID)
                .IsRequired();

            builder.HasOne(u => u.UpdateByUser)
                .WithMany($"UpdateBy{typeof(TEntity).Name}s")
                .HasForeignKey(u => u.UpdateByUserID)
                .HasConstraintName($"FK_{typeof(TEntity).Name}s_UpdateBy{typeof(TEntity).Name}s")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
