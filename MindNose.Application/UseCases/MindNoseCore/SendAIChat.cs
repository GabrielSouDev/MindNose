using MindNose.Domain.Exceptions;
using MindNose.Domain.IAChat;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Nodes;
using MindNose.Domain.Interfaces.UseCases.MindNoseCore;
using MindNose.Domain.Results;

namespace MindNose.Application.UseCases.MindNoseCore;

public class SendAIChat : ISendAIChat
{
    private readonly IOpenRouterService _openRouterService;

    public SendAIChat(IOpenRouterService openRouterService)
    {
        _openRouterService = openRouterService;
    }

    public async Task<string> ExecuteAsync(ChatRequest request)
    {
        try
        {
            var response = await _openRouterService.SendAIChatAsync(request);
            return response;
        }
        catch (Exception)
        {
            return "Houve um erro no envio de sua mensagem, por favor tente novamente!";
        }
    }
}
