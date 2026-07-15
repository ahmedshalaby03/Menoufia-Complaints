namespace ComplaintSystem.Domain.Entities;

public class Notification
{
    public int Id { get; set; }
    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public int? ComplaintId { get; set; }
    public Complaint? Complaint { get; set; }

    public string Message { get; set; } = default!;
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
