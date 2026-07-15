using ComplaintSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ComplaintSystem.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Complaint> Complaints => Set<Complaint>();
    public DbSet<ComplaintAttachment> ComplaintAttachments => Set<ComplaintAttachment>();
    public DbSet<ComplaintStatusHistory> ComplaintStatusHistories => Set<ComplaintStatusHistory>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Governorate> Governorates => Set<Governorate>();
    public DbSet<Center> Centers => Set<Center>();
    public DbSet<Sector> Sectors => Set<Sector>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<GovernmentEntity> GovernmentEntities => Set<GovernmentEntity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
