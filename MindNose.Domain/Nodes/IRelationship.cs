namespace MindNose.Domain.Nodes
{
    public interface IRelationship
    {
        public long Id { get; set; }
        public string ElementId { get; set; }
        public long StartNodeId { get; set; }
        public string StartNodeElementId { get; set; }
        public long EndNodeId { get; set; }
        public string EndNodeElementId { get; set; }
        public string Type { get; set; }
    }
}