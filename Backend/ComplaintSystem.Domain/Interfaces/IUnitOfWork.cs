using ComplaintSystem.Domain.Entities;

namespace ComplaintSystem.Domain.Interfaces;

public interface IUnitOfWork
{
    IRepository<Complaint> Complaints { get; }

    // بيرجع الشكوى مع كل الـ navigation properties (Governorate, Center, Sector, Service,
    // GovernmentEntity, CreatedByUser, AssignedToUser, Attachments, StatusHistory) - عشان الـ mapping لـ ComplaintDetailsDto
    Task<Complaint?> GetComplaintDetailsAsync(int id);
    IRepository<ComplaintAttachment> Attachments { get; }
    IRepository<ComplaintStatusHistory> StatusHistories { get; }
    IRepository<Notification> Notifications { get; }
    IRepository<Governorate> Governorates { get; }
    IRepository<Center> Centers { get; }
    IRepository<Sector> Sectors { get; }
    IRepository<Service> Services { get; }
    IRepository<GovernmentEntity> GovernmentEntities { get; }

    Task<int> SaveChangesAsync();
}
