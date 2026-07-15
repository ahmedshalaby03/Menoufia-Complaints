namespace ComplaintSystem.Domain.Entities;

public class Center
{
    public int Id { get; set; }
    public string NameAr { get; set; } = default!;

    public int GovernorateId { get; set; }
    public Governorate Governorate { get; set; } = default!;
}
