using MindNose.Domain.Enums;

namespace MindNose.Domain.Entities;

public class Message : BaseEntity
{
    public Guid ConversationGuideId { get; set; }
    public ConversationGuide ConversationGuide { get; set; } = default!;
    public string Text { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public MessageOrigin Origin { get; set; } = MessageOrigin.System;
}