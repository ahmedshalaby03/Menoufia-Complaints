namespace ComplaintSystem.Application.DTOs.Lookups;

public class LookupDto
{
    public int Id { get; set; }
    public string NameAr { get; set; } = default!;
}

public class ServiceLookupDto : LookupDto
{
    public int SectorId { get; set; }
}

public class CenterLookupDto : LookupDto
{
    public int GovernorateId { get; set; }
}
