using MindNose.Domain.Entities.Chat;
using MindNose.Domain.IAChat;

namespace MindNose.Domain.Interfaces.UseCases.Chat;

public interface IOpenChat
{
    Task<ConversationGuide?> ExecuteAsync(Guid id);
}