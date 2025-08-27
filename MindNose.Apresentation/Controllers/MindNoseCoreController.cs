using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindNose.Domain;
using MindNose.Domain.Exceptions;
using MindNose.Domain.Nodes;
using MindNose.Domain.Interfaces.UseCases;
using MindNose.Domain.Request;
using MindNose.Application.UseCases;

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

    [HttpPost("Get")]
    public async Task<IActionResult> GetLinks(LinksRequest request)
    {
        try
        {
            var link = await _getLinks.ExecuteAsync(request);
            return Ok(link);
        }
        catch (LinkNotFoundException)
        {
            return NotFound("Node not found!");
        }
    }

    [HttpPost("Post")]
    public async Task<IActionResult> CreateOrGetLinks(LinksRequest request)
    {
        try
        {
            var link = await _createOrGetLinksUseCase.ExecuteAsync(request);
            return Ok(link);
        }
        catch (LinkNotFoundOrCreatedException) { 
            return NotFound("Node not created!");
        }
    }
}
//var response = @"{""id"":""gen-1745213066-fwpTsuO3AOIqFnxVBZqL"",""provider"":""Together"",""model"":""meta-llama/llama-3.3-70binstruct"",""object"":""chat.completion"",""created"":1745213066,""choices""[{""logprobs"":null,""finish_reason"":""stop"",""native_finish_reason"":""stop"",""index"":0,""message"":{""role"":""assistant"",""content"":""```\n{\n \""Term\"": \""crawler\"",\n  \""Summary\"": \""Um crawler, também conhecido como spider ou robô de busca, é um programa de computador que navega pelainternet para indexar e coletar informações de sites e páginas web. No contexto da programação, os crawlers são frequentemente utilizados emdesenvolvimento web para coletar dados, monitorar mudanças em sites e testar a acessibilidade de páginas.\"",\n  \""WeigthCategoryToTerm\"": 0.8,\n \""RelatedTerms\"": [\n    {\n      \""Term\"": \""web scraping\"",\n      \""WeigthCategoryToTerm\"": 0.7,\n      \""WeigthTermToTerm\"": 0.9\n    },\n   {\n      \""Term\"": \""indexação\"",\n      \""WeigthCategoryToTerm\"": 0.6,\n      \""WeigthTermToTerm\"": 0.8\n    },\n    {\n      \""Term\"":\""busca\"",\n      \""WeigthCategoryToTerm\"": 0.5,\n      \""WeigthTermToTerm\"": 0.7\n    }\n  ]\n\n```"",""refusal"":null,""reasoning"":null}}],""usage"":{""prompt_tokens"":379,""completion_tokens"":234,""total_tokens"":613}}";