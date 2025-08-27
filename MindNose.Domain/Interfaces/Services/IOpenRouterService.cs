using MindNose.Domain.CMDs;
using MindNose.Domain.Request;
using MindNose.Domain.TermResults;

namespace MindNose.Domain.Interfaces.Services
{
    public interface IOpenRouterService
    {
        Task<TermResult> CreateTermResult(LinksRequest request);
        Task<string> SubmitPrompt(Prompt prompt);
    }
}