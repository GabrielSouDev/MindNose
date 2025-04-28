namespace RelationalGraph.Domain.Node;

public class RelationshipProperties : IProperties
{
    public string StartNode { get; set; } = string.Empty;
    public string EndNode { get; set; } = string.Empty;
    public double WeigthStartToEnd { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}