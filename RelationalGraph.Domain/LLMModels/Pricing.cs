namespace RelationalGraph.Domain.LLMModels;
public class Pricing
{
    public string Prompt { get; set; } = string.Empty;
    public string Completion { get; set; } = string.Empty;
    public string Request { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Web_Search { get; set; } = string.Empty;
    public string Internal_Reasoning { get; set; } = string.Empty;
    public string Input_Cache_Read { get; set; } = string.Empty;
    public string Input_Cache_Write { get; set; } = string.Empty;
}
