namespace ComplaintSystem.Domain.Entities;

// الخدمة - مرتبطة بقطاع
public class Service
{
    public int Id { get; set; }
    public string NameAr { get; set; } = default!;

    public int SectorId { get; set; }
    public Sector Sector { get; set; } = default!;
}
