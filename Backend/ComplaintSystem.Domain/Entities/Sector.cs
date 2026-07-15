namespace ComplaintSystem.Domain.Entities;

// القطاع
public class Sector
{
    public int Id { get; set; }
    public string NameAr { get; set; } = default!;

    public ICollection<Service> Services { get; set; } = new List<Service>();
}
