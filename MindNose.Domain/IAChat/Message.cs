using MindNose.Domain.Enums;

namespace MindNose.Domain.IAChat;
public class Message
{
    public string? Text { get; set; } = string.Empty;
    public IAChatOrigin Origin { get; set; } = IAChatOrigin.System;
}