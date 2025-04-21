using RelationalGraph.Domain.Common;

namespace RelationalGraph.Domain.Entities
{
    public class TermNode : AuditableEntity
    {
        public int Id { get; set; }
        public string Term { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
    }
}