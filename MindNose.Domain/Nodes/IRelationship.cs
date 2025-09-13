namespace MindNose.Domain.Nodes
{
    public interface IRelationship
    {
        public string ElementId { get; set; }
        public string StartNodeElementId { get; set; }
        public string EndNodeElementId { get; set; }
        public string Type { get; set; }
        public IRelationshipProperties Properties { get; set; }
        public int PathId { get; set; }
    }
}