namespace ComplaintSystem.Application.Interfaces;

public interface INotificationService
{
    Task NotifyAsync(string userId, string message, int? complaintId = null);
}
