using ComplaintSystem.Application.DTOs.Complaints;

namespace ComplaintSystem.Application.Interfaces;

public interface IRagDuplicateCheckService
{
    // بيولد embedding للوصف وبيقارنه بكل الشكاوى المفتوحة (مش مغلقة)
    Task<DuplicateCheckResultDto> CheckForDuplicatesAsync(string description, CancellationToken ct = default);
}
