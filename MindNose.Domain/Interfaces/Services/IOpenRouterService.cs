using MindNose.Domain.CMDs;
using MindNose.Domain.Request;
using MindNose.Domain.Results;

namespace MindNose.Domain.Interfaces.Services
{
    public interface IOpenRouterService
    {
        Task<TermResult> CreateTermResultAsync(LinksRequest request);
        Task<string> SubmitPromptAsync(Prompt prompt, string llmModel);
    }
}