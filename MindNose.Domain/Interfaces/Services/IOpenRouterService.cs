using MindNose.Domain.CMDs;
using MindNose.Domain.TermResults;

namespace MindNose.Domain.Interfaces.Services
{
    public interface IOpenRouterService
    {
        Task<TermResult> CreateTermResult(string category, string term);
        Task<string> SubmitPrompt(Prompt prompt);
    }
}