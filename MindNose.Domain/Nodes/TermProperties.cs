namespace MindNose.Domain.Nodes;

public class TermProperties : IProperties
{
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int TotalTokens { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
