using RelationalGraph.Domain.CMDs;
using RelationalGraph.Domain.TermResult;

namespace RelationalGraph.Application.Interfaces.Services
{
    public interface IOpenRouterService
    {
        Task<TermResult> CreateTermResult(string category, string term);
        Task<string> SubmitPrompt(Prompt prompt);
    }
}