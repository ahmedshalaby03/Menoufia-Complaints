using ComplaintSystem.Application.Common;
using ComplaintSystem.Application.DTOs.Complaints;

namespace ComplaintSystem.Application.Interfaces;

public interface IComplaintService
{
    Task<DuplicateCheckResultDto> CheckDuplicatesAsync(string description);
    Task<ComplaintDetailsDto> CreateAsync(CreateComplaintRequest request, List<(Stream content, string fileName, string contentType)> files);
    Task<ComplaintDetailsDto> GetByIdAsync(int id);
    Task<PagedResult<ComplaintListItemDto>> GetListAsync(ComplaintFilterRequest filter);
    Task<PagedResult<ComplaintListItemDto>> GetInboxAsync(ComplaintFilterRequest filter);
    Task UpdateStatusAsync(int id, UpdateComplaintStatusRequest request);
    Task AssignAsync(int id, AssignComplaintRequest request);
}
