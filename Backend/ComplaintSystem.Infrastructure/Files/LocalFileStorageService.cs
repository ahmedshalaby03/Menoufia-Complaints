using ComplaintSystem.Application.Interfaces;

namespace ComplaintSystem.Infrastructure.Files;

// تخزين محلي بسيط - ممكن تستبدلها بـ Azure Blob / S3 لاحقًا (Infrastructure layer معزولة عشان كدا)
public class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;

    public LocalFileStorageService()
    {
        _basePath = Path.Combine(AppContext.BaseDirectory, "Uploads");
        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> SaveAsync(Stream content, string fileName, int complaintId)
    {
        var complaintFolder = Path.Combine(_basePath, complaintId.ToString());
        Directory.CreateDirectory(complaintFolder);

        var safeFileName = $"{Guid.NewGuid()}_{Path.GetFileName(fileName)}";
        var fullPath = Path.Combine(complaintFolder, safeFileName);

        await using var fileStream = new FileStream(fullPath, FileMode.Create);
        await content.CopyToAsync(fileStream);

        return $"/uploads/{complaintId}/{safeFileName}";
    }
}
