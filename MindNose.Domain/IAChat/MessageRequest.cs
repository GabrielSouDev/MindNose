using MindNose.Domain.Enums;

namespace MindNose.Domain.IAChat;
public class MessageRequest
{
    public string? Text { get; set; } = string.Empty;
    public MessageOrigin Origin { get; set; } = MessageOrigin.System;
}