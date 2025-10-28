using System.Text.Json.Serialization;

namespace MindNose.Domain.LLMModels;
public class Pricing
{
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; } = string.Empty;

    [JsonPropertyName("completion")]
    public string Completion { get; set; } = string.Empty;

    [JsonPropertyName("request")]
    public string Request { get; set; } = string.Empty;

    [JsonPropertyName("image")]
    public string Image { get; set; } = string.Empty;

    [JsonPropertyName("web_search")]
    public string WebSearch { get; set; } = string.Empty;

    [JsonPropertyName("internal_reasoning")]
    public string InternalReasoning { get; set; } = string.Empty;

    [JsonPropertyName("input_cache_read")]
    public string InputCacheRead { get; set; } = string.Empty;

    [JsonPropertyName("input_cache_write")]
    public string InputCacheWrite { get; set; } = string.Empty;

    [JsonPropertyName("audio")]
    public string Audio { get; set; } = string.Empty;

}
