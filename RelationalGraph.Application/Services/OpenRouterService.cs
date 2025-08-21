using RelationalGraph.Application.Interfaces.Clients;
using RelationalGraph.Application.Interfaces.Services;
using RelationalGraph.Application.Operations;
using RelationalGraph.Domain.CMDs;
using RelationalGraph.Domain.TermResult;

namespace RelationalGraph.Application.Services
{
    public class OpenRouterService : IOpenRouterService
    {
        private readonly IOpenRouterClient _openRouterClient;

        public OpenRouterService(IOpenRouterClient openRouterClient)
        {
            _openRouterClient = openRouterClient;
        }

        public async Task<TermResult> CreateTermResult(string category, string term)
        {
            Prompt prompt = PromptFactory.NewKnowledgeNode(category, term);

            var response = await SubmitPrompt(prompt);

            var TermObject = response.TermResultDeserializer();

            return TermObject;
        }

        public async Task<string> SubmitPrompt(Prompt prompt)
        {
            var result = await _openRouterClient.EnviarPrompt(prompt);
            return result;
        }
    }
}
