using ComplaintSystem.Application.DTOs.Complaints;
using ComplaintSystem.Application.Interfaces;
using ComplaintSystem.Domain.Enums;
using ComplaintSystem.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ComplaintSystem.Application.Services;

public class RagDuplicateCheckService : IRagDuplicateCheckService
{
    private readonly IUnitOfWork _uow;
    private readonly IEmbeddingProvider _embeddingProvider;
    private readonly IConfiguration _configuration;

    private const double SimilarityThreshold = 0.83;
    private const int MaxMatches = 5;

    private static readonly ComplaintStatus[] ExcludedStatuses =
        { ComplaintStatus.Rejected };

    public RagDuplicateCheckService(IUnitOfWork uow, IEmbeddingProvider embeddingProvider)
    {
        _uow = uow;
        _embeddingProvider = embeddingProvider;
    }

    public async Task<DuplicateCheckResultDto> CheckForDuplicatesAsync(string description, CancellationToken ct = default)
    {
        if (!_configuration.GetValue<bool>("Rag:Enabled"))
            return new DuplicateCheckResultDto { HasPossibleDuplicates = false, Matches = new() };

        var queryEmbedding = await _embeddingProvider.GetEmbeddingAsync(description, ct);

        var candidates = _uow.Complaints.Query()
            .Where(c => c.EmbeddingJson != null && !ExcludedStatuses.Contains(c.Status))
            .Select(c => new { c.Id, c.ComplaintNumber, c.Subject, c.Description, c.Status, c.EmbeddingJson })
            .ToList();

        var matches = new List<DuplicateMatchDto>();

        foreach (var c in candidates)
        {
            var embedding = JsonSerializer.Deserialize<float[]>(c.EmbeddingJson!);
            if (embedding == null) continue;

            var score = CosineSimilarity(queryEmbedding, embedding);
            if (score >= SimilarityThreshold)
            {
                matches.Add(new DuplicateMatchDto
                {
                    ComplaintId = c.Id,
                    ComplaintNumber = c.ComplaintNumber,
                    Subject = c.Subject,
                    Description = c.Description,
                    Status = c.Status,
                    SimilarityScore = Math.Round(score, 3)
                });
            }
        }

        return new DuplicateCheckResultDto
        {
            HasPossibleDuplicates = matches.Any(),
            Matches = matches.OrderByDescending(m => m.SimilarityScore).Take(MaxMatches).ToList()
        };
    }

    public static double CosineSimilarity(float[] a, float[] b)
    {
        if (a.Length != b.Length) return 0;
        double dot = 0, magA = 0, magB = 0;
        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }
        if (magA == 0 || magB == 0) return 0;
        return dot / (Math.Sqrt(magA) * Math.Sqrt(magB));
    }
}
