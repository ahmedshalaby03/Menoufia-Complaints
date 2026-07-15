using ComplaintSystem.Domain.Enums;

namespace ComplaintSystem.Application.DTOs.Complaints;

public class DuplicateCheckResultDto
{
    public bool HasPossibleDuplicates { get; set; }
    public List<DuplicateMatchDto> Matches { get; set; } = new();
}

public class DuplicateMatchDto
{
    public int ComplaintId { get; set; }
    public string ComplaintNumber { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string Description { get; set; } = default!;
    public ComplaintStatus Status { get; set; }
    public double SimilarityScore { get; set; } // 0..1
}
