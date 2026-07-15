namespace ComplaintSystem.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveAsync(Stream content, string fileName, int complaintId);
}
