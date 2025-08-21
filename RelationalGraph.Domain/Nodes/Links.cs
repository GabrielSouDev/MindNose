namespace RelationalGraph.Domain.Nodes;

public class Links
{
    public List<Node> Nodes { get; set; } = new();
    public List<Relationship> Relationships { get; set; } = new();
}
