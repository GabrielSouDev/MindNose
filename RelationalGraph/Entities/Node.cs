using RelationalGraph.Domain.Common;

namespace RelationalGraph.Domain.Entities
{
    public class Node : AuditableEntity
    {
        public int Id { get; set; }
        public string Term { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;

        public ICollection<Link> InputLinks { get; set; } = new List<Link>();
        public ICollection<Link> OutputLinks { get; set; } = new List<Link>();

        public ICollection<NodeCategory> NodeCategories { get; set; } = new List<NodeCategory>();
    }
}