using MindNose.Application.Services;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Services;

namespace MindNose.Apresentation.Extensions;

public static class ServicesExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEmbeddingService, EmbedingService>();
        builder.Services.AddSingleton<IModelsStorageService, ModelsStorageService>();
        builder.Services.AddScoped<INeo4jService, Neo4jService>();
        builder.Services.AddScoped<IOpenRouterService, OpenRouterService>();
        builder.Services.AddSingleton<ICategoryService, CategoryService>();
        builder.Services.AddScoped<UserService>();
    }
}