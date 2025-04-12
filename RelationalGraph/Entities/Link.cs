using RelationalGraph.Domain.Common;

namespace RelationalGraph.Domain.Entities
{
    public class Link : AuditableEntity
    {
        public int Id { get; set; }

        public int SourceId { get; set; }
        public Node Source { get; set; }

        public int TargetId { get; set; }
        public Node Target { get; set; }

        public float Weight { get; set; }
    }
}
