using MindNose.Domain.CMDs;
using MindNose.Domain.IAChat;
using MindNose.Domain.Request;
using MindNose.Domain.Results;

namespace MindNose.Domain.Interfaces.Services
{
    public interface IOpenRouterService
    {
        Task<LinksResult> CreateCategoryResult(string category);
        Task<LinksResult> CreateTermResultAsync(LinksRequest request);
        Task<(string, Results.Usage)> SendAIChatAsync(MessageRequest request);
        Task<string> SubmitPromptAsync(Prompt prompt, string llmModel);
    }
}