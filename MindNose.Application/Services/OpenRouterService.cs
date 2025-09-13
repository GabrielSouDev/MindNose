using MindNose.Domain.Interfaces.Clients;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Operations;
using MindNose.Domain.CMDs;
using MindNose.Domain.Results;
using MindNose.Domain.Request;

namespace MindNose.Domain.Services
{
    public class OpenRouterService : IOpenRouterService
    {
        private readonly IOpenRouterClient _openRouterClient;

        public OpenRouterService(IOpenRouterClient openRouterClient)
        {
            _openRouterClient = openRouterClient;
        }

        public async Task<TermResult> CreateTermResultAsync(LinksRequest request)
        {

            Prompt prompt = PromptFactory.NewTermResult(request);

            var response = await SubmitPromptAsync(prompt, request.LLMModel);

            var termResult = response.TermResultDeserializer();

            return termResult;
        }

        public async Task<string> SubmitPromptAsync(Prompt prompt, string llmModel)
        {
            var result = await _openRouterClient.EnviarPromptAsync(prompt, llmModel);
            return result;
        }
    }
}
