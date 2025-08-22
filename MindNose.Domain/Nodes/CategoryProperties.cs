namespace MindNose.Domain.Nodes
{
    public class CategoryProperties : IProperties
    {
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}