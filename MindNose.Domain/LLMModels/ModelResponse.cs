using System.Text.Json.Serialization;

namespace MindNose.Domain.LLMModels;
public class ModelResponse
{
    [JsonPropertyName("data")]
    public List<Model> Data { get; set; }

}
