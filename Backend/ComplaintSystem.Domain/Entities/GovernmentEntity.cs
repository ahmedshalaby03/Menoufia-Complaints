namespace ComplaintSystem.Domain.Entities;

// الكيان الحكومي المسؤول عن معالجة الشكوى (بلدية شبين الكوم، الصحة العامة بالمنوفية...)
public class GovernmentEntity
{
    public int Id { get; set; }
    public string NameAr { get; set; } = default!;
    public bool IsActive { get; set; } = true;

    public ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();
}
