using System.Text.Json.Serialization;

namespace MindNose.Domain.OpenAIEmbedding;

public class EmbeddingResponse
{
    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("data")]
    public List<EmbeddingData> Data { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("usage")]
    public Usage Usage { get; set; }

}