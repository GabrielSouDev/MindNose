using Microsoft.AspNetCore.Http;
using MindNose.Domain.Entities.Chat;
using MindNose.Domain.Exceptions;
using MindNose.Domain.IAChat;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Interfaces.UseCases.Chat;
using MindNose.Domain.Interfaces.UseCases.MindNoseCore;
using MindNose.Domain.Nodes;
using MindNose.Domain.Results;

namespace MindNose.Application.UseCases.Chat;

public class DeleteChat : IDeleteChat
{
    private readonly ConversationGuideRepository _guideRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteChat(ConversationGuideRepository guideRepository, IHttpContextAccessor httpContextAccessor)
    {
        _guideRepository = guideRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task ExecuteAsync(Guid id)
    {
        var guide = await _guideRepository.GetByIdAsync(id);

        if(guide is not null)
        {
            await _guideRepository.RemoveAsync(guide);
        }
        else
        {
            throw new ArgumentException("Guide not Found!");
        }
    }
}
