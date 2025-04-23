namespace RelationalGraph.Domain.Node;

public class Graph
{
    public List<Node<TProperties>> Nodes { get; set; } = new();
    public List<Relationship<TProperties>> Relationships { get; set; } = new();

}
