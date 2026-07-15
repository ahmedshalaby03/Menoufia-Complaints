using Microsoft.AspNetCore.Identity;

namespace ComplaintSystem.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = default!;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Complaint> CreatedComplaints { get; set; } = new List<Complaint>();
    public ICollection<Complaint> AssignedComplaints { get; set; } = new List<Complaint>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
