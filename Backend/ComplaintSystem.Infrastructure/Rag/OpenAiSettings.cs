namespace ComplaintSystem.Infrastructure.Rag;

public class OpenAiSettings
{
    public string ApiKey { get; set; } = default!;
    public string EmbeddingModel { get; set; } = "text-embedding-3-small";
}
