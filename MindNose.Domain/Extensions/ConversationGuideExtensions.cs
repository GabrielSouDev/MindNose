
using MindNose.Domain.Entities.Chat;

namespace MindNose.Domain.Extensions;

public static class ConversationGuideExtensions
{
    public static ConversationGuideDTO MapConversationGuideDTO(this ConversationGuide guide)
    {
        var messagesDTO = guide.Messages.Select(m => new MessageDTO()
        {
            Id = m.Id,
            CreatedAt = m.CreatedAt,
            LastModified = m.LastModified,
            Model = m.Model,
            Origin = m.Origin,
            OutputMode = m.OutputMode,
            Text = m.Text
        }).ToList();

        Console.WriteLine("*/-/*-123**- Iniciado */-/*-123**- ");
        foreach (var message in guide.Messages)
        {
            Console.WriteLine(message.Text);
        }
        Console.WriteLine("*/-/*-123**- Finalizado */-/*-123**- ");
        return new ConversationGuideDTO()
        {
            Id = guide.Id,
            ActualModel = guide.ActualModel,
            CreatedAt = guide.CreatedAt,
            LastModified = guide.LastModified,
            Messages = messagesDTO
        };
    }
}