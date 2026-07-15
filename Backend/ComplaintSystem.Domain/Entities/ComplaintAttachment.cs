namespace ComplaintSystem.Domain.Entities;

public class ComplaintAttachment
{
    public int Id { get; set; }
    public int ComplaintId { get; set; }
    public Complaint Complaint { get; set; } = default!;

    public string FileName { get; set; } = default!;
    public string FilePath { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public long FileSizeBytes { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
