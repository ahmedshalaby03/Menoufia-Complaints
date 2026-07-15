using ComplaintSystem.Application.DTOs.Reports;
using ComplaintSystem.Application.Interfaces;
using ComplaintSystem.Domain.Enums;
using ComplaintSystem.Domain.Interfaces;

namespace ComplaintSystem.Application.Services;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _uow;

    public ReportService(IUnitOfWork uow) => _uow = uow;

    public async Task<ReportsSummaryDto> GetReportsAsync()
    {
        var all = _uow.Complaints.Query().ToList();
        var entities = (await _uow.GovernmentEntities.GetAllAsync()).ToList();

        var total = all.Count;
        var closed = all.Count(c => c.Status == ComplaintStatus.Closed);
        var pending = all.Count(c => c.Status is ComplaintStatus.New or ComplaintStatus.Assigned or ComplaintStatus.UnderInvestigation or ComplaintStatus.UnderReview);
        var rejected = all.Count(c => c.Status == ComplaintStatus.Rejected);

        var resolved = all.Where(c => c.ClosedAt.HasValue).ToList();
        var avgResponseDays = resolved.Any() ? resolved.Average(c => (c.ClosedAt!.Value - c.CreatedAt).TotalDays) : 0;

        var topPriority = all
            .GroupBy(c => c.Priority)
            .OrderByDescending(g => g.Count())
            .FirstOrDefault()?.Key.ToString();

        var entityPerformance = entities.Select(e =>
        {
            var entityComplaints = all.Where(c => c.GovernmentEntityId == e.Id).ToList();
            var eTotal = entityComplaints.Count;
            var eClosed = entityComplaints.Count(c => c.Status == ComplaintStatus.Closed);
            var ePending = entityComplaints.Count(c => c.Status is ComplaintStatus.New or ComplaintStatus.Assigned or ComplaintStatus.UnderInvestigation or ComplaintStatus.UnderReview);
            return new EntityPerformanceDto
            {
                EntityName = e.NameAr,
                TotalCount = eTotal,
                ClosedCount = eClosed,
                PendingCount = ePending,
                CompletionRatePercent = eTotal == 0 ? 0 : Math.Round(eClosed * 100.0 / eTotal, 1)
            };
        }).ToList();

        return new ReportsSummaryDto
        {
            CompletionRatePercent = total == 0 ? 0 : Math.Round(closed * 100.0 / total, 1),
            PendingCount = pending,
            ClosedCount = closed,
            TotalCount = total,
            RejectedCount = rejected,
            RejectionRatePercent = total == 0 ? 0 : Math.Round(rejected * 100.0 / total, 1),
            TopPriority = topPriority,
            AverageResponseTimeDays = Math.Round(avgResponseDays, 1),
            PriorityDistribution = all.GroupBy(c => c.Priority.ToString()).ToDictionary(g => g.Key, g => g.Count()),
            StatusDistribution = all.GroupBy(c => c.Status.ToString()).ToDictionary(g => g.Key, g => g.Count()),
            EntityPerformance = entityPerformance
        };
    }
}
