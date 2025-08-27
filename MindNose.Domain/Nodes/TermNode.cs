using System.Text.Json.Serialization;

namespace MindNose.Domain.Nodes;
public class TermNode : INode
{
    public long Id { get; set; }
    public ICollection<string> Label { get; set; } = new List<string>();
    public TermProperties Properties { get; set; } = default!;
    public string ElementId { get; set; } = string.Empty;
}