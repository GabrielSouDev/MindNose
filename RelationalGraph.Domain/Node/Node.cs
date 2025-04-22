namespace RelationalGraph.Application.DTO;

public class Node
{
    public long Id { get; set; }
    public string Label { get; set; }
    public Dictionary<string, object> Properties { get; set; }
    public long ElementId { get; set; }
}