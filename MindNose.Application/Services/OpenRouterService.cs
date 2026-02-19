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
        private readonly PromptFactory _promptFactory;

        public OpenRouterService(IOpenRouterClient openRouterClient, PromptFactory promptFactory)
        {
            _openRouterClient = openRouterClient;
            _promptFactory = promptFactory;
        }

        public async Task<LinksResult> CreateCategoryResult(string category)
        {
            var linksResult = new LinksResult()
            {
                Category = new CategoryResult()
                {
                    Title = category
                },
            };

            var prompt = _promptFactory.Node.NewCategoryResult(category);
            var response = await SubmitPromptAsync(prompt);

            var (categoryResultPartial, usage) = response.CategoryResultDeserializer();

            linksResult.Category.Definition = categoryResultPartial.Definition;
            linksResult.Category.Function = categoryResultPartial.Function;
            linksResult.Usage = usage;

            return linksResult;
        }

        public async Task<LinksResult> CreateTermResultAsync(LinksRequest request)
        {
            var termResult = new LinksResult()
            {
                Category = new CategoryResult()
                {
                    Title = request.Category
                },
                Term = new TermResult()
                {
                    Title = request.Term,
                    CreatedAt = DateTime.UtcNow
                }
            };

            Prompt prompt = _promptFactory.Node.NewTermResult(request);
            var response = await SubmitPromptAsync(prompt, request.LLMModel);
            var (termResultParcial, usage) = response.TermResultDeserializer();

            termResult.Usage = usage;
            termResult.Term.CanonicalDefinition = termResultParcial.CanonicalDefinition;
            termResult.Term.ConceptualCategory = termResultParcial.ConceptualCategory;
            termResult.Term.MainFunction = termResultParcial.MainFunction;

            if (termResultParcial.RelatedTerms is null) return termResult;

            prompt = _promptFactory.Node.NewRelatedTermSummaries(termResult.Category.Title, termResultParcial.RelatedTerms);
            response = await SubmitPromptAsync(prompt, request.LLMModel);

            var (relatedTermResult, usageRelatedTerms) = response.RelatedTermResultDeserializer(termResult);

            foreach (var relTerm in relatedTermResult)
            {
                var term = new RelatedTermResult()
                {
                    Title = relTerm.Title,
                    CanonicalDefinition = relTerm.CanonicalDefinition,
                    ConceptualCategory = relTerm.ConceptualCategory,
                    MainFunction = relTerm.MainFunction,
                    CreatedAt = DateTime.UtcNow
                };

                termResult.RelatedTerms.Add(term);
            }

            return termResult;
        }

        public async Task<string> SendAIChatAsync(ChatRequest request)
        {
            Prompt prompt = _promptFactory.Chat.SendAIChat(request);

            var response = await SubmitPromptAsync(prompt, request.Model);

            var (responseString, usage) = response.ExtractContentAndUsage();

            return responseString;
        }

        public async Task<string> SubmitPromptAsync(Prompt prompt, string llmModel = "qwen/qwen-turbo")
        {
            var result = await _openRouterClient.EnviarPromptAsync(prompt, llmModel);
            return result;
        }
    }
}
