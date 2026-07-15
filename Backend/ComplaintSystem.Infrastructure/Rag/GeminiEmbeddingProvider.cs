using ComplaintSystem.Application.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace ComplaintSystem.Infrastructure.Rag;

public class GeminiEmbeddingProvider : IEmbeddingProvider
{
    private readonly HttpClient _httpClient;
    private readonly GeminiSettings _settings;

    public GeminiEmbeddingProvider(HttpClient httpClient, IOptions<GeminiSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _httpClient.BaseAddress ??= new Uri("https://generativelanguage.googleapis.com/v1beta/");
    }

    public async Task<float[]> GetEmbeddingAsync(string text, CancellationToken ct = default)
    {
        var url = $"models/{_settings.EmbeddingModel}:embedContent?key={_settings.ApiKey}";

        var response = await _httpClient.PostAsJsonAsync(url, new
        {
            model = $"models/{_settings.EmbeddingModel}",
            content = new { parts = new[] { new { text } } }
        }, ct);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: ct);
        var vector = json.GetProperty("embedding").GetProperty("values")
            .EnumerateArray()
            .Select(x => x.GetSingle())
            .ToArray();

        return vector;
    }
}