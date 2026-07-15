using ComplaintSystem.Domain.Enums;
using ComplaintSystem.Application.Common;

namespace ComplaintSystem.Application.DTOs.Complaints;

public class ComplaintFilterRequest : PagedRequest
{
    public ComplaintStatus? Status { get; set; }
    public PriorityLevel? Priority { get; set; }
    public string? SearchTerm { get; set; }
    public bool OnlyMine { get; set; } = false;
}
