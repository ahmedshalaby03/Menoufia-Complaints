using ComplaintSystem.Domain.Enums;

namespace ComplaintSystem.Application.DTOs.Complaints;

public class ComplaintListItemDto
{
    public int Id { get; set; }
    public string ComplaintNumber { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string Description { get; set; } = default!;
    public PriorityLevel Priority { get; set; }
    public ComplaintStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
