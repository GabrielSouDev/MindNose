using MindNose.Application.UseCases.MindNoseCore;
using MindNose.Domain.Interfaces.UseCases.MindNoseCore;

namespace MindNose.Apresentation.Extensions.UseCases;

public static class MindNoseCoreUseCaseExtensions
{
    public static void AdMindNoseCoreUseCase(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IGetLinks, GetLinks>();
        builder.Services.AddScoped<IGetOrCreateLinks, GetOrCreateLinks>();
        builder.Services.AddScoped<ISendAIChat, SendAIChat>();
    }
}