namespace ComplaintSystem.Domain.Enums;

public enum ComplaintStatus
{
    New = 1,                // جديدة
    Assigned = 2,           // معينة
    UnderInvestigation = 3, // قيد التحقيق
    UnderReview = 4,        // قيد المراجعة
    Closed = 5,             // مغلقة (تم الحل)
    Rejected = 6            // مقفولة (مرفوضة/بدون حل)
}
