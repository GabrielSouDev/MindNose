using System.Text.Json.Serialization;

namespace MindNose.Domain.OpenAIEmbedding;

public class Usage
{
    [JsonPropertyName("prompt_tokens")]
    public int? PromptTokens { get; set; }

    [JsonPropertyName("total_tokens")]
    public int? TotalTokens { get; set; }

}
