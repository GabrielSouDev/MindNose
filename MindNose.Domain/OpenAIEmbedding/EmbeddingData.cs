using System.Text.Json.Serialization;

namespace MindNose.Domain.OpenAIEmbedding;

public class EmbeddingData
{
    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;

    [JsonPropertyName("index")]
    public int? Index { get; set; }

    [JsonPropertyName("embedding")]
    public List<double> Embedding { get; set; } = new();

}
