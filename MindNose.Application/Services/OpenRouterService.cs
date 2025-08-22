using MindNose.Domain.Interfaces.Clients;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Operations;
using MindNose.Domain.CMDs;
using MindNose.Domain.TermResults;

namespace MindNose.Domain.Services
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
