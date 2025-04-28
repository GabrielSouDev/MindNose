namespace RelationalGraph.Domain.Node;

public class Link
{
    public List<Node> Nodes { get; set; } = new();
    public List<Relationship> Relationships { get; set; } = new();
}
