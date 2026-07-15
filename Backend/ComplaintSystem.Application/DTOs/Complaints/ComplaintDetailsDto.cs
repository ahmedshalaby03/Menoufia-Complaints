using ComplaintSystem.Domain.Enums;

namespace ComplaintSystem.Application.DTOs.Complaints;

public class ComplaintDetailsDto
{
    public int Id { get; set; }
    public string ComplaintNumber { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string Description { get; set; } = default!;

    public ComplaintSource Source { get; set; }
    public string GovernorateName { get; set; } = default!;
    public string CenterName { get; set; } = default!;
    public string? District { get; set; }

    public ComplaintType Type { get; set; }
    public ComplaintClassification Classification { get; set; }
    public string SectorName { get; set; } = default!;
    public string ServiceName { get; set; } = default!;

    public bool IsInternalComplaint { get; set; }
    public bool IsAffectedWorkerComplaint { get; set; }
    public string? Profession { get; set; }
    public string? JobSector { get; set; }
    public InsuranceType? Insurance { get; set; }
    public DateTime? WorkStopDate { get; set; }

    public PriorityLevel Priority { get; set; }
    public ComplaintStatus Status { get; set; }
    public string? GovernmentEntityName { get; set; }

    public string CreatedByName { get; set; } = default!;
    public string? AssignedToName { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }

    public List<AttachmentDto> Attachments { get; set; } = new();
    public List<StatusHistoryDto> StatusHistory { get; set; } = new();
}

public class AttachmentDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = default!;
    public string FilePath { get; set; } = default!;
}

public class StatusHistoryDto
{
    public ComplaintStatus? OldStatus { get; set; }
    public ComplaintStatus NewStatus { get; set; }
    public string? Notes { get; set; }
    public string ChangedByName { get; set; } = default!;
    public DateTime ChangedAt { get; set; }
}
