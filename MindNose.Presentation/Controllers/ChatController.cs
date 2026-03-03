using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindNose.Domain.Consts;
using MindNose.Domain.Entities.Chat;
using MindNose.Domain.Exceptions;
using MindNose.Domain.Extensions;
using MindNose.Domain.IAChat;
using MindNose.Domain.Interfaces.UseCases.Chat;
using System.Runtime.CompilerServices;

namespace MindNose.Apresentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Poly.UserOrAdmin)]
public class ChatController : ControllerBase
{
    private readonly ISendAIMessage _sendAIMessage;
    private readonly IGetGuidesDisplayByUserId _getGuidesDisplayByUserId;
    private readonly IOpenChat _openChat;
    private readonly INewChat _newChat;
    private readonly IDeleteChat _deleteChat;

    public ChatController(ISendAIMessage sendAIMessage, IGetGuidesDisplayByUserId getGuidesDisplayByUserId, IOpenChat openChat, INewChat newChat, IDeleteChat deleteChat)
    {
        _sendAIMessage = sendAIMessage;
        _getGuidesDisplayByUserId = getGuidesDisplayByUserId;
        _openChat = openChat;
        _newChat = newChat;
        _deleteChat = deleteChat;
    }

    [HttpGet("GetList")]
    public async Task<IActionResult> GetGuideDisplayList()
    {
        try
        {
            var response = await _getGuidesDisplayByUserId.ExecuteAsync();

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest($"Não foi possivel realizar sua requisição, porfavor tente novamente! Erro: {ex.GetType().Name}");
        }
    }

    [HttpPost("New")]
    public async Task<IActionResult> NewChatAsync()
    {
        try
        {
            var guide = await _newChat.ExecuteAsync();

            return Ok(guide.MapConversationGuideDTO());
        }
        catch (Exception ex)
        {
            return BadRequest($"Não foi possivel realizar sua requisição, porfavor tente novamente! Erro: {ex.GetType().Name}");
        }
    }

    [HttpGet("Open/{id}")]
    public async Task<IActionResult> OpenChatAsync(Guid id)
    {
        try
        {
            var guide = await _openChat.ExecuteAsync(id);

            if (guide is null) return Ok();

            return Ok(guide.MapConversationGuideDTO());
        }
        catch (Exception ex)
        {
            return NotFound($"Não foi possivel realizar sua requisição, porfavor tente novamente! Erro: {ex.GetType().Name}");
        }
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteChatAsync(Guid id)
    {
        try
        {
            await _deleteChat.ExecuteAsync(id);

            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound($"Não foi possivel realizar sua requisição, porfavor tente novamente! Erro: {ex.GetType().Name}");
        }
    }

    [HttpPost("SendAIMessage")]
    public async Task<IActionResult> SendAIMessageAsync([FromBody] MessageRequest request)
    {
        try
        {
            var response = await _sendAIMessage.ExecuteAsync(request);

            return Ok(response);
        }
        catch (OpenRouterRequestException)
        {
            return BadRequest("Não foi possivel realizar sua requisição, porfavor tente novamente!");
        }
    }
}
//var response = @"{""id"":""gen-1745213066-fwpTsuO3AOIqFnxVBZqL"",""provider"":""Together"",""model"":""meta-llama/llama-3.3-70binstruct"",""object"":""chat.completion"",""created"":1745213066,""choices""[{""logprobs"":null,""finish_reason"":""stop"",""native_finish_reason"":""stop"",""index"":0,""message"":{""role"":""assistant"",""content"":""```\n{\n \""Term\"": \""crawler\"",\n  \""Summary\"": \""Um crawler, também conhecido como spider ou robô de busca, é um programa de computador que navega pelainternet para indexar e coletar informações de sites e páginas web. No contexto da programação, os crawlers são frequentemente utilizados emdesenvolvimento web para coletar dados, monitorar mudanças em sites e testar a acessibilidade de páginas.\"",\n  \""WeigthCategoryToTerm\"": 0.8,\n \""RelatedTerms\"": [\n    {\n      \""Term\"": \""web scraping\"",\n      \""WeigthCategoryToTerm\"": 0.7,\n      \""WeigthTermToTerm\"": 0.9\n    },\n   {\n      \""Term\"": \""indexação\"",\n      \""WeigthCategoryToTerm\"": 0.6,\n      \""WeigthTermToTerm\"": 0.8\n    },\n    {\n      \""Term\"":\""busca\"",\n      \""WeigthCategoryToTerm\"": 0.5,\n      \""WeigthTermToTerm\"": 0.7\n    }\n  ]\n\n```"",""refusal"":null,""reasoning"":null}}],""usage"":{""prompt_tokens"":379,""completion_tokens"":234,""total_tokens"":613}}";