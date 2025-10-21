using MindNose.Domain.CMDs;

namespace MindNose.Domain.Interfaces.Clients;
public interface IOpenRouterClient
{
    Task<string> EnviarPromptAsync(Prompt prompt);
    Task<string> EnviarPromptAsync(Prompt prompt, string llmModel);
}