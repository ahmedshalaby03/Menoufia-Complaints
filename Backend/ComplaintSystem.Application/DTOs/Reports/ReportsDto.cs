namespace ComplaintSystem.Application.DTOs.Reports;

public class ReportsSummaryDto
{
    public double CompletionRatePercent { get; set; }
    public int PendingCount { get; set; }
    public int ClosedCount { get; set; }
    public int TotalCount { get; set; }

    public Dictionary<string, int> PriorityDistribution { get; set; } = new();
    public Dictionary<string, int> StatusDistribution { get; set; } = new();

    public double RejectionRatePercent { get; set; }
    public int RejectedCount { get; set; }
    public string? TopPriority { get; set; }
    public double AverageResponseTimeDays { get; set; }

    public List<EntityPerformanceDto> EntityPerformance { get; set; } = new();
}

public class EntityPerformanceDto
{
    public string EntityName { get; set; } = default!;
    public double CompletionRatePercent { get; set; }
    public int PendingCount { get; set; }
    public int ClosedCount { get; set; }
    public int TotalCount { get; set; }
}
