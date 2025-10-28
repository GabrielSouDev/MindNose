using MindNose.Domain.IAChat;

namespace MindNose.Domain.Interfaces.UseCases.MindNoseCore;

public interface ISendAIChat
{
    Task<string> ExecuteAsync(ChatRequest request);
}