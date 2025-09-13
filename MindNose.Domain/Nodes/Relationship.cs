namespace MindNose.Domain.Nodes;

public class Relationship : IRelationship
{
    public string ElementId { get; set; } = string.Empty;
    public string StartNodeElementId { get; set; } = string.Empty;
    public string EndNodeElementId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public IRelationshipProperties Properties { get; set; } = default!;
    public int PathId { get; set; }
}
