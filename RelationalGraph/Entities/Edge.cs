using RelationalGraph.Domain.Common;

namespace RelationalGraph.Domain.Entities
{
    public class Edge : AuditableEntity
    {
        public int Id { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        public float Weight { get; set; }
        public string Label { get; set; } = string.Empty;
    }
}
