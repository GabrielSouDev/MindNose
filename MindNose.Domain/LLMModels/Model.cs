using System.Text.Json.Serialization;

namespace MindNose.Domain.LLMModels;
public class Model
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("canonical_slug")]
    public string CanonicalSlug { get; set; }

    [JsonPropertyName("hugging_face_id")]
    public string HuggingFaceId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("created")]
    public int? Created { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("context_length")]
    public int? ContextLength { get; set; }

    [JsonPropertyName("architecture")]
    public Architecture Architecture { get; set; }

    [JsonPropertyName("pricing")]
    public Pricing Pricing { get; set; }

    [JsonPropertyName("top_provider")]
    public TopProvider TopProvider { get; set; }

    [JsonPropertyName("per_request_limits")]
    public object? PerRequestLimits { get; set; }

    [JsonPropertyName("supported_parameters")]
    public List<string> SupportedParameters { get; set; }

    [JsonPropertyName("default_parameters")]
    public object? DefaultParameters { get; set; }
}
