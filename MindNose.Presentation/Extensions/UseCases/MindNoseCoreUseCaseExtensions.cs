using MindNose.Application.UseCases.Chat;
using MindNose.Application.UseCases.MindNoseCore;
using MindNose.Domain.Interfaces.UseCases.Chat;
using MindNose.Domain.Interfaces.UseCases.MindNoseCore;

namespace MindNose.Apresentation.Extensions.UseCases;

public static class ChatUseCaseExtensions
{
    public static void AddMindNoseCoreUseCase(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IGetLinks, GetLinks>();
        builder.Services.AddScoped<IGetOrCreateLinks, GetOrCreateLinks>();
    }
}