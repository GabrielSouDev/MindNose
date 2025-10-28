namespace MindNose.Domain.IAChat;

public class ChatRequest
{
    public List<ElementsHeader>? ElementsHeader { get; set; } = new();
    public Message Message { get; set; } = new();
    public string Model { get; set; } = string.Empty;
}