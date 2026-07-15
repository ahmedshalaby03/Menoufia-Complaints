using ComplaintSystem.Application.DTOs.Dashboard;

namespace ComplaintSystem.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetSummaryAsync();
}
