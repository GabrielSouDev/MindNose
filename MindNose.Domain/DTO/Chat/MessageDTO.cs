using MindNose.Domain.Enums;

namespace MindNose.Domain.Entities.Chat;

public class MessageDTO
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModified { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public OutputMode OutputMode { get; set; } = OutputMode.Conversational;
    public MessageOrigin Origin { get; set; } = MessageOrigin.System;
}
