using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RelationalGraph.Application;
using RelationalGraph.Application.Interfaces.Services;
using RelationalGraph.Application.Operations;

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
            //consultar neo4j

            //se existir retornar existente

            //senão faz abaixo

            Prompt prompt = Prompt.CreateKnowledgeNode(category,term);
            string response = await _openRouterService.SearchFirstLevel(prompt);
            //var response = @"{""id"":""gen-1745213066-fwpTsuO3AOIqFnxVBZqL"",""provider"":""Together"",""model"":""meta-llama/llama-3.3-70b-instruct"",""object"":""chat.completion"",""created"":1745213066,""choices"":[{""logprobs"":null,""finish_reason"":""stop"",""native_finish_reason"":""stop"",""index"":0,""message"":{""role"":""assistant"",""content"":""```\n{\n  \""Term\"": \""crawler\"",\n  \""Summary\"": \""Um crawler, também conhecido como spider ou robô de busca, é um programa de computador que navega pela internet para indexar e coletar informações de sites e páginas web. No contexto da programação, os crawlers são frequentemente utilizados em desenvolvimento web para coletar dados, monitorar mudanças em sites e testar a acessibilidade de páginas.\"",\n  \""WeigthCategoryToTerm\"": 0.8,\n  \""RelatedTerms\"": [\n    {\n      \""Term\"": \""web scraping\"",\n      \""WeigthCategoryToTerm\"": 0.7,\n      \""WeigthTermToTerm\"": 0.9\n    },\n    {\n      \""Term\"": \""indexação\"",\n      \""WeigthCategoryToTerm\"": 0.6,\n      \""WeigthTermToTerm\"": 0.8\n    },\n    {\n      \""Term\"": \""busca\"",\n      \""WeigthCategoryToTerm\"": 0.5,\n      \""WeigthTermToTerm\"": 0.7\n    }\n  ]\n}\n```"",""refusal"":null,""reasoning"":null}}],""usage"":{""prompt_tokens"":379,""completion_tokens"":234,""total_tokens"":613}}";

            var result = await _neo4jService.CreateKnowledgeNode(response);
            //cria nó no neo4j
            return Ok(result);
        }
    }
}
