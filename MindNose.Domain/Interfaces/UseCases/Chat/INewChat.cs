using MindNose.Domain.Entities.Chat;
using MindNose.Domain.IAChat;

namespace MindNose.Domain.Interfaces.UseCases.Chat;

public interface INewChat
{
    Task<ConversationGuide> ExecuteAsync();
}