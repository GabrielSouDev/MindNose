using MindNose.Domain.CMDs;

namespace MindNose.Domain.Interfaces.Clients;
public interface IOpenRouterClient
{
    Task<string> EnviarPrompt(Prompt prompt);
}