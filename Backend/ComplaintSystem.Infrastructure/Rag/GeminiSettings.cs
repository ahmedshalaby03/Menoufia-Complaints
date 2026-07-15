namespace ComplaintSystem.Infrastructure.Rag;

public class GeminiSettings
{
    public string ApiKey { get; set; } = default!;
    public string EmbeddingModel { get; set; } = "gemini-embedding-001";
}