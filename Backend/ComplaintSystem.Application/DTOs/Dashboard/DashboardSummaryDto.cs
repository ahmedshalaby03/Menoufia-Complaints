namespace ComplaintSystem.Application.DTOs.Dashboard;

public class DashboardSummaryDto
{
    public int ClosedCount { get; set; }        // مغلقة
    public int InProgressCount { get; set; }    // قيد المعالجة
    public int NewCount { get; set; }            // جديدة
    public int TotalCount { get; set; }          // إجمالي الشكاوى

    public double CompletionRatePercent { get; set; } // نسبة الإنجاز
    public int RejectedCount { get; set; }             // المعلقة/المرفوضة
    public double AverageResponseTimeDays { get; set; }

    public PriorityDistributionDto PriorityDistribution { get; set; } = new();
    public List<RecentComplaintDto> RecentComplaints { get; set; } = new();
}

public class PriorityDistributionDto
{
    public int Urgent { get; set; }
    public int High { get; set; }
    public int Medium { get; set; }
    public int Low { get; set; }
}

public class RecentComplaintDto
{
    public int Id { get; set; }
    public string ComplaintNumber { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Priority { get; set; } = default!;
    public string Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
