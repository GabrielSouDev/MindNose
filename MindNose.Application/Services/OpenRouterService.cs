using MindNose.Domain.CMDs;
using MindNose.Domain.IAChat;
using MindNose.Domain.Interfaces.Clients;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.OpenAIEmbedding;
using MindNose.Domain.Operations;
using MindNose.Domain.Request;
using MindNose.Domain.Results;
using Neo4j.Driver;

namespace MindNose.Domain.Services
{
    public class OpenRouterService : IOpenRouterService
    {
        private readonly IOpenRouterClient _openRouterClient;

        public OpenRouterService(IOpenRouterClient openRouterClient)
        {
            _openRouterClient = openRouterClient;
        }

        public async Task<LinksResult> CreateCategoryResult(string category)
        {
            var prompt = PromptFactory.NewCategoryResult(category);

            var response = await SubmitPromptAsync(prompt);

            var linksResult = response.CategoryResultDeserializer();
            return linksResult;
        }

        public async Task<LinksResult> CreateTermResultAsync(LinksRequest request)
        {
            Prompt prompt = PromptFactory.NewTermResult(request);

            var response = await SubmitPromptAsync(prompt, request.LLMModel);
            var termResult = response.TermResultDeserializer();

            prompt = PromptFactory.NewRelatedTermSummaries(termResult);

            response = await SubmitPromptAsync(prompt, request.LLMModel);
            termResult = response.RelatedTermResultDeserializer(termResult);

            return termResult;
        }

        public async Task<string> SendAIChatAsync(ChatRequest request)
        {
            Prompt prompt = PromptFactory.SendAIChat(request);

            var response = await SubmitPromptAsync(prompt, request.Model);

            var (responseString, usage) = response.ChatAIDeserializer();

            return responseString;
        }

        public async Task<string> SubmitPromptAsync(Prompt prompt, string llmModel = "qwen/qwen-turbo")
        {
            var result = await _openRouterClient.EnviarPromptAsync(prompt, llmModel);
            return result;
        }
    }
}
