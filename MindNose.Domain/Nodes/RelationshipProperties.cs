namespace MindNose.Domain.Nodes;

public class RelationshipProperties : IRelationshipProperties
{
    public double WeigthStartToEnd { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}