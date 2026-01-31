namespace MindNose.Domain.Entities;

public class ConversationGuide : BaseEntity
{
    public Guid UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; } = default!;
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<Chunk> Chunks { get; set; } = new List<Chunk>();
    public string ActualModel { get; set; } = string.Empty;
}