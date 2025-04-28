
using RelationalGraph.Application.Operations;

namespace RelationalGraph.Application.Interfaces.Services
{
    public interface IOpenRouterService
    {
        Task<string> SubmitPrompt(Prompt prompt);
    }
}