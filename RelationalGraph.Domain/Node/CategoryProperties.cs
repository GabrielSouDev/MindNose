namespace RelationalGraph.Domain.Node
{
    public class CategoryProperties : IProperties
    {
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}