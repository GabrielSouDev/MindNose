using RelationalGraph.Application.Interfaces.Clients;
using RelationalGraph.Application.Interfaces.Services;
using RelationalGraph.Application.Operations;

namespace RelationalGraph.Application.Services
{
    public class OpenRouterService : IOpenRouterService
    {
        private readonly IOpenRouterClient _openRouterClient;

        public OpenRouterService(IOpenRouterClient openRouterClient)
        {
            _openRouterClient = openRouterClient;
        }

        public async Task<string> SubmitPrompt(Prompt prompt)
        {
            var result = await _openRouterClient.EnviarPrompt(prompt);
            return result;
        }
    }
}
