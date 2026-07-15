using ComplaintSystem.Application.DTOs.Reports;

namespace ComplaintSystem.Application.Interfaces;

public interface IReportService
{
    Task<ReportsSummaryDto> GetReportsAsync();
}
