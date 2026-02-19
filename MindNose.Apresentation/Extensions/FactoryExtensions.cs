using MindNose.Domain.Operations;

namespace MindNose.Apresentation.Extensions;

public static class FactoryExtensions
{
    public static void AddFactories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<PromptFactory>();
        builder.Services.AddScoped<PromptChatFactory>();
        builder.Services.AddScoped<PromptNodeFactory>();
    }
}