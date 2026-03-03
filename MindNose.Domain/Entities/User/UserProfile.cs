using MindNose.Domain.Entities.Chat;

namespace MindNose.Domain.Entities.User;

public class UserProfile : BaseEntity
{
    public ICollection<ConversationGuide> ConversationGuides { get; set; } = new List<ConversationGuide>();
    public bool IsActive { get; set; } = true;
    public DateTime? DeletedAt { get; set; }

    public void Deactivate()
    {
        IsActive = false;
        DeletedAt = DateTime.UtcNow;
    }
}