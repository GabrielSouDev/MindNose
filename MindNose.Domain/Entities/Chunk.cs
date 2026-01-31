namespace MindNose.Domain.Entities;

public class Chunk : BaseEntity
{
    public Guid ConversationGuideId { get; set; }
    public ConversationGuide ConversationGuide { get; set; } = default!;
    public string Text { get; set; } = string.Empty;
    public ICollection<float> Embedding { get; set; } = new List<float>();
}