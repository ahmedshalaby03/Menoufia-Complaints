using ComplaintSystem.Domain.Entities;
using ComplaintSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComplaintSystem.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Complaints = new Repository<Complaint>(context);
        Attachments = new Repository<ComplaintAttachment>(context);
        StatusHistories = new Repository<ComplaintStatusHistory>(context);
        Notifications = new Repository<Notification>(context);
        Governorates = new Repository<Governorate>(context);
        Centers = new Repository<Center>(context);
        Sectors = new Repository<Sector>(context);
        Services = new Repository<Service>(context);
        GovernmentEntities = new Repository<GovernmentEntity>(context);
    }

    public IRepository<Complaint> Complaints { get; }
    public IRepository<ComplaintAttachment> Attachments { get; }
    public IRepository<ComplaintStatusHistory> StatusHistories { get; }
    public IRepository<Notification> Notifications { get; }
    public IRepository<Governorate> Governorates { get; }
    public IRepository<Center> Centers { get; }
    public IRepository<Sector> Sectors { get; }
    public IRepository<Service> Services { get; }
    public IRepository<GovernmentEntity> GovernmentEntities { get; }

    public Task<Complaint?> GetComplaintDetailsAsync(int id) =>
        _context.Complaints
            .AsNoTracking()
            .Include(c => c.Governorate)
            .Include(c => c.Center)
            .Include(c => c.Sector)
            .Include(c => c.Service)
            .Include(c => c.GovernmentEntity)
            .Include(c => c.CreatedByUser)
            .Include(c => c.AssignedToUser)
            .Include(c => c.Attachments)
            .Include(c => c.StatusHistory).ThenInclude(h => h.ChangedByUser)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}
