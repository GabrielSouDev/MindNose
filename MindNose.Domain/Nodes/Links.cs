using MindNose.Domain.Results;

namespace MindNose.Domain.Nodes;

public class Links
{
    public List<INode> Nodes { get; set; } = new();
    public List<IRelationship> Relationships { get; set; } = new();
    public Usage Usage { get; set; } = new();

}
