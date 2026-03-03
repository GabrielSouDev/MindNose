using MindNose.Domain.Entities.Chat;

namespace MindNose.Domain.Interfaces.UseCases.Chat;

public interface IGetGuidesDisplayByUserId
{
    Task<List<ConversationGuideDisplay>?> ExecuteAsync();
}