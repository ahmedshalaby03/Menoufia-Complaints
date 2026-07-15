using ComplaintSystem.Domain.Enums;

namespace ComplaintSystem.Application.DTOs.Complaints;

public class CreateComplaintRequest
{
    public string Subject { get; set; } = default!;
    public string Description { get; set; } = default!;

    public ComplaintSource Source { get; set; }
    public int GovernorateId { get; set; }
    public int CenterId { get; set; }
    public string? District { get; set; }

    public ComplaintType Type { get; set; }
    public ComplaintClassification Classification { get; set; }

    public int SectorId { get; set; }
    public int ServiceId { get; set; }

    public bool IsInternalComplaint { get; set; }

    public bool IsAffectedWorkerComplaint { get; set; }
    public string? Profession { get; set; }
    public string? JobSector { get; set; }
    public InsuranceType? Insurance { get; set; }
    public DateTime? WorkStopDate { get; set; }

    public PriorityLevel Priority { get; set; }
    public int? GovernmentEntityId { get; set; }

    // لو الموظف أكد إنه عايز يكمل رغم وجود شكوى مشابهة (بعد ما شاف التحذير)
    public bool ConfirmDespiteDuplicateWarning { get; set; } = false;
}
