using ComplaintSystem.Application.DTOs.Dashboard;
using ComplaintSystem.Application.Interfaces;
using ComplaintSystem.Domain.Enums;
using ComplaintSystem.Domain.Interfaces;

namespace ComplaintSystem.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public DashboardService(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync()
    {
        var query = _uow.Complaints.Query();

        // الموظف بيشوف بس الشكاوى اللي هو دخلها، الأدمن بيشوف الكل
        if (!_currentUser.IsAdmin)
            query = query.Where(c => c.CreatedByUserId == _currentUser.UserId);

        var all = query.ToList();
        var total = all.Count;
        var closed = all.Count(c => c.Status == ComplaintStatus.Closed);
        var newC = all.Count(c => c.Status == ComplaintStatus.New);
        var inProgress = all.Count(c => c.Status is ComplaintStatus.Assigned or ComplaintStatus.UnderInvestigation or ComplaintStatus.UnderReview);
        var rejected = all.Count(c => c.Status == ComplaintStatus.Rejected);

        var resolved = all.Where(c => c.ClosedAt.HasValue).ToList();
        var avgResponseDays = resolved.Any()
            ? resolved.Average(c => (c.ClosedAt!.Value - c.CreatedAt).TotalDays)
            : 0;

        return new DashboardSummaryDto
        {
            ClosedCount = closed,
            InProgressCount = inProgress,
            NewCount = newC,
            TotalCount = total,
            RejectedCount = rejected,
            CompletionRatePercent = total == 0 ? 0 : Math.Round(closed * 100.0 / total, 1),
            AverageResponseTimeDays = Math.Round(avgResponseDays, 1),
            PriorityDistribution = new PriorityDistributionDto
            {
                Urgent = all.Count(c => c.Priority == PriorityLevel.Urgent),
                High = all.Count(c => c.Priority == PriorityLevel.High),
                Medium = all.Count(c => c.Priority == PriorityLevel.Medium),
                Low = all.Count(c => c.Priority == PriorityLevel.Low)
            },
            RecentComplaints = all
                .OrderByDescending(c => c.CreatedAt)
                .Take(5)
                .Select(c => new RecentComplaintDto
                {
                    Id = c.Id,
                    ComplaintNumber = c.ComplaintNumber,
                    Subject = c.Subject,
                    Description = c.Description,
                    Priority = c.Priority.ToString(),
                    Status = c.Status.ToString(),
                    CreatedAt = c.CreatedAt
                }).ToList()
        };
    }
}
