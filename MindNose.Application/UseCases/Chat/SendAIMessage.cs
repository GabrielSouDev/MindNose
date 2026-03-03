using Message = MindNose.Domain.Entities.Chat.Message;
using MindNose.Domain.IAChat;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Interfaces.UseCases.Chat;
using MindNose.Domain.Enums;

namespace MindNose.Application.UseCases.Chat;

public class SendAIMessage : ISendAIMessage
{
    private readonly IOpenRouterService _openRouterService;
    private readonly MessageRepository _messageRepository;

    public SendAIMessage(IOpenRouterService openRouterService, MessageRepository messageRepository)
    {
        _openRouterService = openRouterService;
        _messageRepository = messageRepository;
    }

    public async Task<string> ExecuteAsync(MessageRequest request)
    {
        try
        {
            var UserMessage = new Message()
            {
                ConversationGuideId = request.ConversationGuideId,
                Model = request.Model,
                Text = request.Message.Text!,
                Origin = request.Message.Origin,
                OutputMode = request.OutputMode
            };
            _messageRepository.Add(UserMessage);

            var (response, usage) = await _openRouterService.SendAIChatAsync(request);

            var AIMessage = new Message()
            {
                ConversationGuideId = request.ConversationGuideId,
                Model = request.Model,
                Text = response,
                Origin = MessageOrigin.System,
                OutputMode = request.OutputMode
            };
            _messageRepository.Add(AIMessage);

            return response;
        }
        catch (Exception)
        {
            return "Houve um erro no envio de sua mensagem, por favor tente novamente!";
        }
    }
}

//TODO: adicionar campos usage e elementsHeader em message e persistir