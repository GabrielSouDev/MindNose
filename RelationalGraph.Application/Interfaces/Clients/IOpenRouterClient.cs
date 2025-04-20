using RelationalGraph.Application.Operations;

namespace RelationalGraph.Application.Interfaces.Clients;
public interface IOpenRouterClient
{
    Task<string> EnviarPrompt(Prompt prompt);
}