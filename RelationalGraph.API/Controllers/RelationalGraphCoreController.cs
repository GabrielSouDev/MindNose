using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RelationalGraph.Application;
using RelationalGraph.Application.Interfaces.Services;
using RelationalGraph.Application.Operations;
using RelationalGraph.Domain.Node;

namespace RelationalGraph.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelationalGraphCoreController : ControllerBase
    {
        private readonly IOpenRouterService _openRouterService;
        private readonly INeo4jService _neo4jService;

        public RelationalGraphCoreController(IOpenRouterService openRouterService, INeo4jService neo4jService)
        {
            _openRouterService = openRouterService;
            _neo4jService = neo4jService;
        }

        [HttpPost("Post/{category}/{term}")]
        public async Task<IActionResult> SearchAndCreateKnowledgeNode(string category, string term)
        {
            category = WordFormat(category);
            term = WordFormat(term);

            Link link = await _neo4jService.NodeIsExists(category, term);//resgata node se existir no neo4j

            if (link is not null)
                return Ok(link);

            Prompt prompt = PromptFactory.NewKnowledgeNode(category, term); //prepara prompt para IA
            string response = await _openRouterService.SubmitPrompt(prompt); //chama IA

            link = await _neo4jService.CreateKnowledgeNode(response); //cria nó no neo4j
            if (link is not null)
                return Ok(link);

            return NotFound("Node not found");
        }
        private string WordFormat(string word)
        {
            return char.ToUpper(word[0]) + word.Substring(1).ToLower();
        }
    }
}
//var response = @"{""id"":""gen-1745213066-fwpTsuO3AOIqFnxVBZqL"",""provider"":""Together"",""model"":""meta-llama/llama-3.3-70binstruct"",""object"":""chat.completion"",""created"":1745213066,""choices""[{""logprobs"":null,""finish_reason"":""stop"",""native_finish_reason"":""stop"",""index"":0,""message"":{""role"":""assistant"",""content"":""```\n{\n \""Term\"": \""crawler\"",\n  \""Summary\"": \""Um crawler, também conhecido como spider ou robô de busca, é um programa de computador que navega pelainternet para indexar e coletar informações de sites e páginas web. No contexto da programação, os crawlers são frequentemente utilizados emdesenvolvimento web para coletar dados, monitorar mudanças em sites e testar a acessibilidade de páginas.\"",\n  \""WeigthCategoryToTerm\"": 0.8,\n \""RelatedTerms\"": [\n    {\n      \""Term\"": \""web scraping\"",\n      \""WeigthCategoryToTerm\"": 0.7,\n      \""WeigthTermToTerm\"": 0.9\n    },\n   {\n      \""Term\"": \""indexação\"",\n      \""WeigthCategoryToTerm\"": 0.6,\n      \""WeigthTermToTerm\"": 0.8\n    },\n    {\n      \""Term\"":\""busca\"",\n      \""WeigthCategoryToTerm\"": 0.5,\n      \""WeigthTermToTerm\"": 0.7\n    }\n  ]\n\n```"",""refusal"":null,""reasoning"":null}}],""usage"":{""prompt_tokens"":379,""completion_tokens"":234,""total_tokens"":613}}";