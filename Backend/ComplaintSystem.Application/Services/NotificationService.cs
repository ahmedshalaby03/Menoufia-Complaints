using ComplaintSystem.Application.Interfaces;
using ComplaintSystem.Domain.Entities;
using ComplaintSystem.Domain.Interfaces;

namespace ComplaintSystem.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _uow;

    public NotificationService(IUnitOfWork uow) => _uow = uow;

    public async Task NotifyAsync(string userId, string message, int? complaintId = null)
    {
        await _uow.Notifications.AddAsync(new Notification
        {
            UserId = userId,
            Message = message,
            ComplaintId = complaintId
        });
        await _uow.SaveChangesAsync();
    }
}
