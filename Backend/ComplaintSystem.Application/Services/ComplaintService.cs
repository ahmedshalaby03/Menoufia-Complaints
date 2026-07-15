using AutoMapper;
using ComplaintSystem.Application.Common;
using ComplaintSystem.Application.DTOs.Complaints;
using ComplaintSystem.Application.Interfaces;
using ComplaintSystem.Domain.Entities;
using ComplaintSystem.Domain.Enums;
using ComplaintSystem.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ComplaintSystem.Application.Services;

public class ComplaintService : IComplaintService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IEmbeddingProvider _embeddingProvider;
    private readonly IRagDuplicateCheckService _duplicateCheckService;
    private readonly ICurrentUserService _currentUser;
    private readonly INotificationService _notificationService;
    private readonly IFileStorageService _fileStorage;
    private readonly IConfiguration _configuration;

    public ComplaintService(
        IUnitOfWork uow,
        IMapper mapper,
        IEmbeddingProvider embeddingProvider,
        IRagDuplicateCheckService duplicateCheckService,
        ICurrentUserService currentUser,
        INotificationService notificationService,
        IFileStorageService fileStorage,
        IConfiguration configuration)
    {
        _uow = uow;
        _mapper = mapper;
        _embeddingProvider = embeddingProvider;
        _duplicateCheckService = duplicateCheckService;
        _currentUser = currentUser;
        _notificationService = notificationService;
        _fileStorage = fileStorage;
        _configuration = configuration;
    }

    public Task<DuplicateCheckResultDto> CheckDuplicatesAsync(string description)
    {
        if (!_configuration.GetValue<bool>("Rag:Enabled"))
            return Task.FromResult(new DuplicateCheckResultDto { HasPossibleDuplicates = false, Matches = new() });

        return _duplicateCheckService.CheckForDuplicatesAsync(description);
    }

    public async Task<ComplaintDetailsDto> CreateAsync(CreateComplaintRequest request, List<(Stream content, string fileName, string contentType)> files)
    {
        var ragEnabled = _configuration.GetValue<bool>("Rag:Enabled");

        if (ragEnabled && !request.ConfirmDespiteDuplicateWarning)
        {
            var dup = await _duplicateCheckService.CheckForDuplicatesAsync(request.Description);
            if (dup.HasPossibleDuplicates)
                throw new DuplicateComplaintException(dup);
        }

        string? embeddingJson = null;
        if (ragEnabled)
        {
            var embedding = await _embeddingProvider.GetEmbeddingAsync(request.Description);
            embeddingJson = JsonSerializer.Serialize(embedding);
        }

        var complaint = new Complaint
        {
            ComplaintNumber = await GenerateComplaintNumberAsync(),
            Subject = request.Subject,
            Description = request.Description,
            Source = request.Source,
            GovernorateId = request.GovernorateId,
            CenterId = request.CenterId,
            District = request.District,
            Type = request.Type,
            Classification = request.Classification,
            SectorId = request.SectorId,
            ServiceId = request.ServiceId,
            IsInternalComplaint = request.IsInternalComplaint,
            IsAffectedWorkerComplaint = request.IsAffectedWorkerComplaint,
            Profession = request.Profession,
            JobSector = request.JobSector,
            Insurance = request.Insurance,
            WorkStopDate = request.WorkStopDate,
            Priority = request.Priority,
            GovernmentEntityId = request.GovernmentEntityId,
            Status = request.GovernmentEntityId.HasValue ? ComplaintStatus.Assigned : ComplaintStatus.New,
            CreatedByUserId = _currentUser.UserId!,
            EmbeddingJson = embeddingJson
        };

        await _uow.Complaints.AddAsync(complaint);
        await _uow.SaveChangesAsync();

        foreach (var file in files)
        {
            var path = await _fileStorage.SaveAsync(file.content, file.fileName, complaint.Id);
            await _uow.Attachments.AddAsync(new ComplaintAttachment
            {
                ComplaintId = complaint.Id,
                FileName = file.fileName,
                FilePath = path,
                ContentType = file.contentType
            });
        }

        await _uow.StatusHistories.AddAsync(new ComplaintStatusHistory
        {
            ComplaintId = complaint.Id,
            OldStatus = null,
            NewStatus = complaint.Status,
            ChangedByUserId = _currentUser.UserId!,
            Notes = "تسجيل الشكوى"
        });

        await _uow.SaveChangesAsync();

        return await GetByIdAsync(complaint.Id);
    }

    public async Task<ComplaintDetailsDto> GetByIdAsync(int id)
    {
        var complaint = await _uow.GetComplaintDetailsAsync(id)
            ?? throw new KeyNotFoundException("الشكوى غير موجودة");

        return _mapper.Map<ComplaintDetailsDto>(complaint);
    }

    public async Task<PagedResult<ComplaintListItemDto>> GetListAsync(ComplaintFilterRequest filter) =>
        await BuildFilteredResultAsync(filter, restrictToCurrentUser: false);

    public async Task<PagedResult<ComplaintListItemDto>> GetInboxAsync(ComplaintFilterRequest filter) =>
        await BuildFilteredResultAsync(filter, restrictToCurrentUser: true);

    private async Task<PagedResult<ComplaintListItemDto>> BuildFilteredResultAsync(ComplaintFilterRequest filter, bool restrictToCurrentUser)
    {
        var query = _uow.Complaints.Query();

        if (restrictToCurrentUser)
            query = query.Where(c => c.AssignedToUserId == _currentUser.UserId);

        if (filter.Status.HasValue) query = query.Where(c => c.Status == filter.Status);
        if (filter.Priority.HasValue) query = query.Where(c => c.Priority == filter.Priority);
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            query = query.Where(c => c.Subject.Contains(filter.SearchTerm) || c.Description.Contains(filter.SearchTerm));

        var total = query.Count();
        var items = query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();

        return new PagedResult<ComplaintListItemDto>
        {
            Items = _mapper.Map<List<ComplaintListItemDto>>(items),
            TotalCount = total,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
        };
    }

    public async Task UpdateStatusAsync(int id, UpdateComplaintStatusRequest request)
    {
        var complaint = await _uow.Complaints.GetByIdAsync(id) ?? throw new KeyNotFoundException("الشكوى غير موجودة");

        var oldStatus = complaint.Status;
        complaint.Status = request.NewStatus;
        complaint.UpdatedAt = DateTime.UtcNow;
        if (request.NewStatus is ComplaintStatus.Closed or ComplaintStatus.Rejected)
            complaint.ClosedAt = DateTime.UtcNow;

        _uow.Complaints.Update(complaint);

        await _uow.StatusHistories.AddAsync(new ComplaintStatusHistory
        {
            ComplaintId = complaint.Id,
            OldStatus = oldStatus,
            NewStatus = request.NewStatus,
            ChangedByUserId = _currentUser.UserId!,
            Notes = request.Notes
        });

        await _uow.SaveChangesAsync();

        if (!string.IsNullOrEmpty(complaint.CreatedByUserId))
            await _notificationService.NotifyAsync(complaint.CreatedByUserId,
                $"تم تحديث حالة الشكوى {complaint.ComplaintNumber} إلى {request.NewStatus}", complaint.Id);
    }

    public async Task AssignAsync(int id, AssignComplaintRequest request)
    {
        var complaint = await _uow.Complaints.GetByIdAsync(id) ?? throw new KeyNotFoundException("الشكوى غير موجودة");

        complaint.GovernmentEntityId = request.GovernmentEntityId ?? complaint.GovernmentEntityId;
        complaint.AssignedToUserId = request.AssignedToUserId;
        if (complaint.Status == ComplaintStatus.New)
            complaint.Status = ComplaintStatus.Assigned;
        complaint.UpdatedAt = DateTime.UtcNow;

        _uow.Complaints.Update(complaint);

        await _uow.StatusHistories.AddAsync(new ComplaintStatusHistory
        {
            ComplaintId = complaint.Id,
            OldStatus = complaint.Status,
            NewStatus = complaint.Status,
            ChangedByUserId = _currentUser.UserId!,
            Notes = "تعيين الشكوى"
        });

        await _uow.SaveChangesAsync();

        if (!string.IsNullOrEmpty(request.AssignedToUserId))
            await _notificationService.NotifyAsync(request.AssignedToUserId,
                $"تم تعيين شكوى جديدة لك: {complaint.ComplaintNumber}", complaint.Id);
    }

    private async Task<string> GenerateComplaintNumberAsync()
    {
        var year = DateTime.UtcNow.Year;
        var countThisYear = _uow.Complaints.Query().Count(c => c.CreatedAt.Year == year);
        return $"COM-{year}-{(countThisYear + 1):D4}";
    }
}

public class DuplicateComplaintException : Exception
{
    public DuplicateCheckResultDto Result { get; }
    public DuplicateComplaintException(DuplicateCheckResultDto result)
        : base("تم العثور على شكاوى مشابهة محتملة - راجع النتائج قبل الحفظ")
    {
        Result = result;
    }
}
