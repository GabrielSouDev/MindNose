using System.Text.Json.Serialization;

namespace MindNose.Domain.LLMModels;
public class Architecture
{
    [JsonPropertyName("modality")]
    public string Modality { get; set; } = string.Empty;

    [JsonPropertyName("input_modalities")]
    public List<string> InputModalities { get; set; } = new();

    [JsonPropertyName("output_modalities")]
    public List<string> OutputModalities { get; set; } = new();

    [JsonPropertyName("tokenizer")]
    public string Tokenizer { get; set; } = string.Empty;

    [JsonPropertyName("instruct_type")]
    public object? InstructType { get; set; }
}
