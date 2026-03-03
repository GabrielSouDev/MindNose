using MindNose.Domain.Enums;

namespace MindNose.Domain.IAChat;

public class MessageRequest
{
    public Guid ConversationGuideId { get; set; }
    public List<ElementsHeader>? ElementsHeader { get; set; } = new();
    public Message Message { get; set; } = new();
    public OutputMode OutputMode { get; set; } = OutputMode.Conversational;
    public string Model { get; set; } = string.Empty;
}