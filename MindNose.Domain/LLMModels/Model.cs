using System.Text.Json.Serialization;

namespace MindNose.Domain.LLMModels;
public class Model
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("canonical_slug")]
    public string CanonicalSlug { get; set; } = string.Empty;

    [JsonPropertyName("hugging_face_id")]
    public string HuggingFaceId { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("created")]
    public int? Created { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("context_length")]
    public int? ContextLength { get; set; }

    [JsonPropertyName("architecture")]
    public Architecture Architecture { get; set; } = new();

    [JsonPropertyName("pricing")]
    public Pricing Pricing { get; set; } = new();

    [JsonPropertyName("top_provider")]
    public TopProvider TopProvider { get; set; } = new();

    [JsonPropertyName("per_request_limits")]
    public object? PerRequestLimits { get; set; }

    [JsonPropertyName("supported_parameters")]
    public List<string> SupportedParameters { get; set; } = new();

    [JsonPropertyName("default_parameters")]
    public object? DefaultParameters { get; set; }
}
