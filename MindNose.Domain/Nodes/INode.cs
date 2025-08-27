namespace MindNose.Domain.Nodes
{
    public interface INode
    {
        public long Id { get; set; }
        public ICollection<string> Label { get; set; }
        public string ElementId { get; set; }
    }
}