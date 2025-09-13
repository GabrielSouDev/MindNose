using System.Text.Json.Serialization;

namespace MindNose.Domain.Nodes;
public class TermNode : INode
{
    public string ElementId { get; set; } = string.Empty;
    public ICollection<string> Label { get; set; } = new List<string>();
    public IProperties Properties { get; set; } = default!;

}