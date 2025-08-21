namespace RelationalGraph.Domain.Nodes;

public class TermProperties : IProperties
{
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public Usage Usage { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
