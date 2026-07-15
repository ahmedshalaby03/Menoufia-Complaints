using ComplaintSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComplaintSystem.Infrastructure.Persistence.Configurations;

public class ComplaintConfiguration : IEntityTypeConfiguration<Complaint>
{
    public void Configure(EntityTypeBuilder<Complaint> builder)
    {
        builder.HasIndex(c => c.ComplaintNumber).IsUnique();
        builder.Property(c => c.ComplaintNumber).HasMaxLength(30).IsRequired();
        builder.Property(c => c.Subject).HasMaxLength(300).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(4000).IsRequired();
        builder.Property(c => c.EmbeddingJson).HasColumnType("nvarchar(max)");

        builder.HasOne(c => c.Governorate).WithMany().HasForeignKey(c => c.GovernorateId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(c => c.Center).WithMany().HasForeignKey(c => c.CenterId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(c => c.Sector).WithMany().HasForeignKey(c => c.SectorId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(c => c.Service).WithMany().HasForeignKey(c => c.ServiceId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(c => c.GovernmentEntity).WithMany(e => e.Complaints).HasForeignKey(c => c.GovernmentEntityId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.CreatedByUser).WithMany(u => u.CreatedComplaints)
            .HasForeignKey(c => c.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(c => c.AssignedToUser).WithMany(u => u.AssignedComplaints)
            .HasForeignKey(c => c.AssignedToUserId).OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Attachments).WithOne(a => a.Complaint).HasForeignKey(a => a.ComplaintId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(c => c.StatusHistory).WithOne(h => h.Complaint).HasForeignKey(h => h.ComplaintId).OnDelete(DeleteBehavior.Cascade);
    }
}
