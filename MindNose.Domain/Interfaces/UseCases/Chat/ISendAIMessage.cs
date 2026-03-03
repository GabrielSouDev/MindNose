using MindNose.Domain.IAChat;

namespace MindNose.Domain.Interfaces.UseCases.Chat;

public interface ISendAIMessage
{
    Task<string> ExecuteAsync(MessageRequest request);
}