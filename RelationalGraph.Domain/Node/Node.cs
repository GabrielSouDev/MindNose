namespace RelationalGraph.Domain.Node;

public class Node
{
    public long Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public Dictionary<string, object> Properties { get; set; } = new();
    public long ElementId { get; set; }
}