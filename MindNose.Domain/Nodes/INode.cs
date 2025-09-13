namespace MindNose.Domain.Nodes
{
    public interface INode
    {
        public ICollection<string> Label { get; set; }
        public string ElementId { get; set; }
        IProperties Properties { get; set; }
    }
}