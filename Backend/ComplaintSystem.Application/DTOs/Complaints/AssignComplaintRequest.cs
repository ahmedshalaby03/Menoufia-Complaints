namespace ComplaintSystem.Application.DTOs.Complaints;

public class AssignComplaintRequest
{
    public int? GovernmentEntityId { get; set; }
    public string? AssignedToUserId { get; set; }
}
