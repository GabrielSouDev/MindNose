using RelationalGraph.Domain.Enums;

namespace RelationalGraph.Domain.Entities
{
    public class NodeAccessLog
    {
        public int Id { get; set; }

        public int NodeId { get; set; }
        public Node Node { get; set; }

        public DateTime AccessedAt { get; set; } = DateTime.UtcNow;

        public LinkContext AccessContext { get; set; }
    }
}
