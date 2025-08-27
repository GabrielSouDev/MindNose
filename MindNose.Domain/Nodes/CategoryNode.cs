namespace MindNose.Domain.Nodes;
public class CategoryNode : INode
{
    public long Id { get; set; }
    public ICollection<string> Label { get; set; } = new List<string>();
    public CategoryProperties Properties { get; set; } = default!;
    public string ElementId { get; set; } = string.Empty;
}