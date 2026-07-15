using ComplaintSystem.Domain.Enums;

namespace ComplaintSystem.Domain.Entities;

public class ComplaintStatusHistory
{
    public int Id { get; set; }
    public int ComplaintId { get; set; }
    public Complaint Complaint { get; set; } = default!;

    public ComplaintStatus? OldStatus { get; set; }
    public ComplaintStatus NewStatus { get; set; }
    public string? Notes { get; set; }

    public string ChangedByUserId { get; set; } = default!;
    public ApplicationUser ChangedByUser { get; set; } = default!;
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}
