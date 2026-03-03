using MindNose.Domain.Entities.User;

namespace MindNose.Domain.Entities.Chat;

public class ConversationGuideDTO
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModified { get; set; }
    public ICollection<MessageDTO> Messages { get; set; } = new List<MessageDTO>();
    public string ActualModel { get; set; } = string.Empty;
}