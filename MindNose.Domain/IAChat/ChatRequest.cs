using MindNose.Domain.Enums;

namespace MindNose.Domain.IAChat;

public class ChatRequest
{
    public List<ElementsHeader>? ElementsHeader { get; set; } = new();
    public MessageRequest Message { get; set; } = new();
    public OutputMode OutputMode { get; set; } = OutputMode.Conversational;
    public string Model { get; set; } = string.Empty;
}