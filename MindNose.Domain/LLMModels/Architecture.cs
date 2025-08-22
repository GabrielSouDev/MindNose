namespace MindNose.Domain.LLMModels;
public class Architecture
{
    public string Modality { get; set; } = string.Empty;
    public List<string> Input_Modalities { get; set; } = new();
    public List<string> Output_Modalities { get; set; } = new();
    public string Tokenizer { get; set; } = string.Empty;
    public string Instruct_Type { get; set; } = string.Empty;
}
