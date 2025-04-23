namespace RelationalGraph.Domain.Node;
public class Node<TProperties>
{
    public long Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public TProperties Properties { get; set; } = default!;
    public long ElementId { get; set; }
}
