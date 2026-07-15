using ComplaintSystem.Domain.Enums;

namespace ComplaintSystem.Application.DTOs.Complaints;

public class UpdateComplaintStatusRequest
{
    public ComplaintStatus NewStatus { get; set; }
    public string? Notes { get; set; }
}
