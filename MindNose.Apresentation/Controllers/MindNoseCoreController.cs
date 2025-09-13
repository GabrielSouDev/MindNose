using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindNose.Application.UseCases;
using MindNose.Domain;
using MindNose.Domain.Exceptions;
using MindNose.Domain.Interfaces.UseCases;
using MindNose.Domain.Nodes;
using MindNose.Domain.Request;
using Neo4j.Driver;
using System.Diagnostics;

namespace MindNose.Apresentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MindNoseCoreController : ControllerBase
{

    private readonly IGetLinks _getLinks;
    private readonly ICreateOrGetLinksUseCase _createOrGetLinksUseCase;

    public MindNoseCoreController(IGetLinks getLinks, ICreateOrGetLinksUseCase relationalGraphService)
    {
        _getLinks = getLinks;
        _createOrGetLinksUseCase = relationalGraphService;
    }

    [HttpPost("GetLink")]
    public async Task<IActionResult> GetLinksAsync(string category, string term, int lengthPath = 1, int limit = 10,int skip = 0)
    {
        var request = new LinksRequest()
        {
            Category = category,
            Term = term,
            LengthPath = lengthPath,
            Limit = limit,
            Skip = skip
        };

        try
        {
            var link = await _getLinks.ExecuteAsync(request);
            return Ok(link);
        }
        catch (LinksNotFoundException)
        {
            return NotFound("Node not found!");
        }
    }

    [HttpPost("CreateOrGetLink")]
    public async Task<IActionResult> CreateOrGetLinksAsync(string category, string term, string llmModel = "mistralai/mistral-small-3.2-24b-instruct")
    {
        var request = new LinksRequest()
        {
            Category = category,
            Term = term,
            LLMModel = llmModel
        }; 

        try
        {
            var link = await _createOrGetLinksUseCase.ExecuteAsync(request);

            return Ok(link);
        }
        catch (LinksNotCreatedException)
        {
            return NotFound("Node not created!");
        }
    }
}
//var response = @"{""id"":""gen-1745213066-fwpTsuO3AOIqFnxVBZqL"",""provider"":""Together"",""model"":""meta-llama/llama-3.3-70binstruct"",""object"":""chat.completion"",""created"":1745213066,""choices""[{""logprobs"":null,""finish_reason"":""stop"",""native_finish_reason"":""stop"",""index"":0,""message"":{""role"":""assistant"",""content"":""```\n{\n \""Term\"": \""crawler\"",\n  \""Summary\"": \""Um crawler, também conhecido como spider ou robô de busca, é um programa de computador que navega pelainternet para indexar e coletar informações de sites e páginas web. No contexto da programação, os crawlers são frequentemente utilizados emdesenvolvimento web para coletar dados, monitorar mudanças em sites e testar a acessibilidade de páginas.\"",\n  \""WeigthCategoryToTerm\"": 0.8,\n \""RelatedTerms\"": [\n    {\n      \""Term\"": \""web scraping\"",\n      \""WeigthCategoryToTerm\"": 0.7,\n      \""WeigthTermToTerm\"": 0.9\n    },\n   {\n      \""Term\"": \""indexação\"",\n      \""WeigthCategoryToTerm\"": 0.6,\n      \""WeigthTermToTerm\"": 0.8\n    },\n    {\n      \""Term\"":\""busca\"",\n      \""WeigthCategoryToTerm\"": 0.5,\n      \""WeigthTermToTerm\"": 0.7\n    }\n  ]\n\n```"",""refusal"":null,""reasoning"":null}}],""usage"":{""prompt_tokens"":379,""completion_tokens"":234,""total_tokens"":613}}";