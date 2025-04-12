using RelationalGraph.Domain.Common;
using RelationalGraph.Domain.Entities;
using RelationalGraph.Domain.Enums;

public class NodeCategory : AuditableEntity
{
    public int NodeId { get; set; }
    public Node Node { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    //public int CreatedByUserId {  get; set; }
    //public User CreatedByUser { get; set; } = string.Empty;
    public LinkContext? SourceType { get; set; }
}
