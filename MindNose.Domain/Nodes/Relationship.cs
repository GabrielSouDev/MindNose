namespace MindNose.Domain.Nodes;

public class Relationship : IRelationship
{
    public long Id { get; set; }
    public string ElementId { get; set; } = string.Empty;
    public long StartNodeId { get; set; }
    public string StartNodeElementId { get; set; } = string.Empty;
    public long EndNodeId { get; set; }
    public string EndNodeElementId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public RelationshipProperties Properties { get; set; } = new();
}
