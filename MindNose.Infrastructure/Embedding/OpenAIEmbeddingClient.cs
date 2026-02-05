using MindNose.Domain.Interfaces.Clients;
using MindNose.Domain.OpenAIEmbedding;
using System.Net.Http.Headers;
using System.Numerics;
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

        var embeddingResponse = JsonSerializer.Deserialize<EmbeddingResponse>(responseBody);

        var rawEmbeddings = embeddingResponse.Data.Select(d => d.Embedding.ToArray()).ToArray();
        return NormalizeEmbeddingsSIMD(rawEmbeddings);
    }

    private static double[][] NormalizeEmbeddingsSIMD(double[][] embeddings)
    {
        var result = new double[embeddings.Length][];
        Parallel.For(0, embeddings.Length, i =>
        {
            var vec = embeddings[i];
            int len = vec.Length;
            result[i] = new double[len];

            double norm = 0;
            int simdSize = Vector<double>.Count;
            int j = 0;

            for (; j <= len - simdSize; j += simdSize)
            {
                var v = new Vector<double>(vec, j);
                norm += Vector.Dot(v, v);
            }

            for (; j < len; j++)
                norm += vec[j] * vec[j];

            norm = Math.Sqrt(norm);

            if (norm > 0)
            {
                var normVec = new Vector<double>(norm);
                j = 0;
                for (; j <= len - simdSize; j += simdSize)
                {
                    var v = new Vector<double>(vec, j);
                    (v / normVec).CopyTo(result[i], j);
                }
                for (; j < len; j++)
                    result[i][j] = vec[j] / norm;
            }
            else
            {
                Array.Copy(vec, result[i], len);
            }
        });
        return result;
    }

    public double CosineSimilaritySIMD(double[] a, double[] b)
    {
        if (a.Length != b.Length) throw new ArgumentException("Tamanho dos vetores necessitam ser iguais!");

        int simdLength = Vector<double>.Count;
        int i = 0;

        var dotVec = Vector<double>.Zero;
        var magAVec = Vector<double>.Zero;
        var magBVec = Vector<double>.Zero;

        for (; i <= a.Length - simdLength; i += simdLength)
        {
            var va = new Vector<double>(a, i);
            var vb = new Vector<double>(b, i);

            dotVec += va * vb;
            magAVec += va * va;
            magBVec += vb * vb;
        }

        double dot = 0, magA = 0, magB = 0;

        dot += Vector.Sum(dotVec); 
        magA += Vector.Sum(magAVec);
        magB += Vector.Sum(magBVec);

        for (; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }

        return dot / (Math.Sqrt(magA) * Math.Sqrt(magB));
    }
}

