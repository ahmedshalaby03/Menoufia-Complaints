namespace ComplaintSystem.Application.Interfaces;

// بيتنفذ في Infrastructure - هيكلم OpenAI API (text-embedding-3-small)
public interface IEmbeddingProvider
{
    Task<float[]> GetEmbeddingAsync(string text, CancellationToken ct = default);
}
