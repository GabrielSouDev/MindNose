namespace RelationalGraph.Domain.Node
{
    public class CategoryProperties : IProperties
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}