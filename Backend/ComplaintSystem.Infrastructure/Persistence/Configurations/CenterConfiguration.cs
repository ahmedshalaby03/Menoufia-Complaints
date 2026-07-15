using ComplaintSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComplaintSystem.Infrastructure.Persistence.Configurations;

public class CenterConfiguration : IEntityTypeConfiguration<Center>
{
    public void Configure(EntityTypeBuilder<Center> builder)
    {
        builder.Property(c => c.NameAr).HasMaxLength(150).IsRequired();
        builder.HasOne(c => c.Governorate).WithMany(g => g.Centers).HasForeignKey(c => c.GovernorateId).OnDelete(DeleteBehavior.Cascade);
    }
}
