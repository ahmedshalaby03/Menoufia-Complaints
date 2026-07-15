using ComplaintSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComplaintSystem.Infrastructure.Persistence.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.Property(s => s.NameAr).HasMaxLength(200).IsRequired();
        builder.HasOne(s => s.Sector).WithMany(sec => sec.Services).HasForeignKey(s => s.SectorId).OnDelete(DeleteBehavior.Cascade);
    }
}
