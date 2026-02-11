using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindNose.Domain.Consts;
using MindNose.Domain.Exceptions;
using MindNose.Domain.Extensions;
using MindNose.Domain.IAChat;
using MindNose.Domain.Interfaces.UseCases.MindNoseCore;
using MindNose.Domain.Request;

namespace MindNose.Apresentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MindNoseCoreController : ControllerBase
{

    private readonly IGetLinks _getLinks;
    private readonly IGetOrCreateLinks _getOrCreateLinks;
    private readonly ISendAIChat _sendAIChat;

    public MindNoseCoreController(IGetLinks getLinks, IGetOrCreateLinks getOrCreateLinksUseCase, ISendAIChat sendAIChat)
    {
        _getLinks = getLinks;
        _getOrCreateLinks = getOrCreateLinksUseCase;
        _sendAIChat = sendAIChat;
    }

    [Authorize(Policy = Poly.UserOrAdmin)]
    [HttpGet("GetLink")]
    public async Task<IActionResult> GetLinksAsync([FromQuery] LinksRequest request)
    {
        try
        {
            var link = await _getLinks.ExecuteAsync(request);
            return Ok(link.LinksToDTO());
        }
        catch (LinksNotFoundException)
        {
            return NotFound("Node not found!");
        }
    }

    [Authorize(Policy = Poly.UserOrAdmin)]
    [HttpPost("GetOrCreateLink")]
    public async Task<IActionResult> GetOrCreateLinksAsync([FromBody] LinksRequest request)
    {
        try
        {
            var link = await _getOrCreateLinks.ExecuteAsync(request);

            return Ok(link.LinksToDTO());
        }
        catch (LinksNotCreatedException)
        {
            return NotFound("Node not created!");
        }
        catch (CategoryNotFoundException)
        { 
            return NotFound("Category not found!"); 
        }
    }

    [Authorize(Policy = Poly.UserOrAdmin)]
    [HttpPost("SendAIChat")]
    public async Task<IActionResult> SendAIChatAsync([FromBody] ChatRequest request)
    {
        try
        {
            var response = await _sendAIChat.ExecuteAsync(request);

            return Ok(response);
        }
        catch (OpenRouterRequestException)
        {
            return NotFound("Não foi possivel realizar sua requisição, porfavor tente novamente!");
        }
    }
}
//var response = @"{""id"":""gen-1745213066-fwpTsuO3AOIqFnxVBZqL"",""provider"":""Together"",""model"":""meta-llama/llama-3.3-70binstruct"",""object"":""chat.completion"",""created"":1745213066,""choices""[{""logprobs"":null,""finish_reason"":""stop"",""native_finish_reason"":""stop"",""index"":0,""message"":{""role"":""assistant"",""content"":""```\n{\n \""Term\"": \""crawler\"",\n  \""Summary\"": \""Um crawler, também conhecido como spider ou robô de busca, é um programa de computador que navega pelainternet para indexar e coletar informações de sites e páginas web. No contexto da programação, os crawlers são frequentemente utilizados emdesenvolvimento web para coletar dados, monitorar mudanças em sites e testar a acessibilidade de páginas.\"",\n  \""WeigthCategoryToTerm\"": 0.8,\n \""RelatedTerms\"": [\n    {\n      \""Term\"": \""web scraping\"",\n      \""WeigthCategoryToTerm\"": 0.7,\n      \""WeigthTermToTerm\"": 0.9\n    },\n   {\n      \""Term\"": \""indexação\"",\n      \""WeigthCategoryToTerm\"": 0.6,\n      \""WeigthTermToTerm\"": 0.8\n    },\n    {\n      \""Term\"":\""busca\"",\n      \""WeigthCategoryToTerm\"": 0.5,\n      \""WeigthTermToTerm\"": 0.7\n    }\n  ]\n\n```"",""refusal"":null,""reasoning"":null}}],""usage"":{""prompt_tokens"":379,""completion_tokens"":234,""total_tokens"":613}}";