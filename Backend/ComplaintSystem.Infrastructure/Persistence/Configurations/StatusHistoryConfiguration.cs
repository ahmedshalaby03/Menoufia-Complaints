using ComplaintSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComplaintSystem.Infrastructure.Persistence.Configurations;

public class StatusHistoryConfiguration : IEntityTypeConfiguration<ComplaintStatusHistory>
{
    public void Configure(EntityTypeBuilder<ComplaintStatusHistory> builder)
    {
        builder.HasOne(h => h.ChangedByUser).WithMany().HasForeignKey(h => h.ChangedByUserId).OnDelete(DeleteBehavior.Restrict);
    }
}
