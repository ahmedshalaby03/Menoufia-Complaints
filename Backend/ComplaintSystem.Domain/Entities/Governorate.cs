namespace ComplaintSystem.Domain.Entities;

public class Governorate
{
    public int Id { get; set; }
    public string NameAr { get; set; } = default!;

    public ICollection<Center> Centers { get; set; } = new List<Center>();
}
