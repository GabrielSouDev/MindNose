using MindNose.Application.UseCases.Chat;
using MindNose.Application.UseCases.MindNoseCore;
using MindNose.Domain.Interfaces.UseCases.Chat;
using MindNose.Domain.Interfaces.UseCases.MindNoseCore;

namespace MindNose.Apresentation.Extensions.UseCases;

public static class MindNoseCoreUseCaseExtensions
{
    public static void AddChatUseCase(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ISendAIMessage, SendAIMessage>();
        builder.Services.AddScoped<IGetGuidesDisplayByUserId, GetGuidesDisplayByUserId>();
        builder.Services.AddScoped<INewChat, NewChat>();
        builder.Services.AddScoped<IOpenChat, OpenChat>();
        builder.Services.AddScoped<IDeleteChat, DeleteChat>();
    }
}