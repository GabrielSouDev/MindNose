using MindNose.Domain.Interfaces.Clients;
using MindNose.Domain.OpenAIEmbedding;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MindNose.Infrastructure.Embedding;

public class OpenAIEmbeddingClient : IEmbeddingClient
{
    private readonly HttpClient _httpClient;
    private readonly string _model = "text-embedding-3-small";

    public OpenAIEmbeddingClient(string apiKey)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.openai.com/v1/")
        };
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
    }

    public async Task<double[][]> GetSentenceEmbeddingAsync(string[] sentenses)
    {
        var payload = new
        {
            input = sentenses,
            model = _model
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("embeddings", content);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine("Resposta da API: " + responseBody);

        var embeddingResponse = JsonSerializer.Deserialize<EmbeddingResponse>(responseBody);

        var rawEmbeddings = embeddingResponse.Data.Select(d => d.Embedding.ToArray()).ToArray();
        return NormalizeEmbeddings(rawEmbeddings);
    }

    private double[][] NormalizeEmbeddings(double[][] embeddings)
    {
        return embeddings.Select(vec =>
        {
            var norm = Math.Sqrt(vec.Sum(x => x * x));
            return norm > 0 ? vec.Select(x => x / norm).ToArray() : vec;
        }).ToArray();
    }

    public float CosineSimilarity(double[] vec1, double[] vec2)
    {
        double dot = vec1.Zip(vec2, (a, b) => a * b).Sum();
        return (float)dot;
    }
}
