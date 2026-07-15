using ComplaintSystem.Domain.Enums;

namespace ComplaintSystem.Domain.Entities;

public class Complaint : BaseEntity
{
    public string ComplaintNumber { get; set; } = default!; // COM-2026-0001

    // بيانات أساسية
    public string Subject { get; set; } = default!;          // موضوع الشكوى
    public string Description { get; set; } = default!;      // وصف مختصر للشكوى

    public ComplaintSource Source { get; set; }               // مصدر الشكوى
    public int GovernorateId { get; set; }
    public Governorate Governorate { get; set; } = default!;
    public int CenterId { get; set; }
    public Center Center { get; set; } = default!;
    public string? District { get; set; }                     // الحي / القرية

    public ComplaintType Type { get; set; }                    // فردية / جماعية
    public ComplaintClassification Classification { get; set; } // تصنيف الشكوى

    public int SectorId { get; set; }                           // القطاع
    public Sector Sector { get; set; } = default!;
    public int ServiceId { get; set; }                           // الخدمة
    public Service Service { get; set; } = default!;

    public bool IsInternalComplaint { get; set; }                // شكوى داخلية (الشاكي موظف بالجهة)

    // شكوى عمالة متضررة
    public bool IsAffectedWorkerComplaint { get; set; }
    public string? Profession { get; set; }                      // المهنة
    public string? JobSector { get; set; }                       // قطاع الوظيفة
    public InsuranceType? Insurance { get; set; }                 // التأمينات
    public DateTime? WorkStopDate { get; set; }                   // تاريخ توقف العمل

    // معالجة
    public PriorityLevel Priority { get; set; }
    public ComplaintStatus Status { get; set; } = ComplaintStatus.New;
    public int? GovernmentEntityId { get; set; }                  // الكيان الحكومي المختار للمعالجة
    public GovernmentEntity? GovernmentEntity { get; set; }

    public string CreatedByUserId { get; set; } = default!;       // الموظف اللي دخل الشكوى
    public ApplicationUser CreatedByUser { get; set; } = default!;
    public string? AssignedToUserId { get; set; }
    public ApplicationUser? AssignedToUser { get; set; }

    public DateTime? ClosedAt { get; set; }

    // RAG - تخزين الـ embedding بتاع الوصف كـ JSON array of floats
    public string? EmbeddingJson { get; set; }

    public ICollection<ComplaintAttachment> Attachments { get; set; } = new List<ComplaintAttachment>();
    public ICollection<ComplaintStatusHistory> StatusHistory { get; set; } = new List<ComplaintStatusHistory>();
}
