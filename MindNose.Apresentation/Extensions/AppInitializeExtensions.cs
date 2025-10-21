using MindNose.Application.Services;
using MindNose.Domain.Interfaces.Clients;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Results;
using MindNose.Infrastructure.HttpClients;
using MindNose.Infrastructure.Persistence;

namespace MindNose.Apresentation.Extensions.Services;

public static class AppInitializeExtensions
{
    public static async Task InitializeAsync(this WebApplication app, bool isLocalEmbedding)
    {
        using (var scope = app.Services.CreateScope())
        {
            var modelsStorageService = scope.ServiceProvider.GetRequiredService<IModelsStorageService>();
            var neo4jClient = scope.ServiceProvider.GetRequiredService<INeo4jClient>();
            var categoryService = scope.ServiceProvider.GetRequiredService<ICategoryService>();

            if (isLocalEmbedding)
            {
                var embeddingClient = scope.ServiceProvider.GetRequiredService<IEmbeddingClient>();
                if (embeddingClient is LocalEmbeddingClient _embeddingClient)
                    await _embeddingClient.InitializeAsync();
            }

            await modelsStorageService.InitializeAsync();

            if (neo4jClient is Neo4jClient _neo4jclient)
                await _neo4jclient.InitializeAsync();

            var categories = await neo4jClient.GetCategories();
            if (categories != null)
            {
                var CategoriesResult = categories.Nodes.Select(c =>
                                new CategoryResult()
                                {
                                    Title = c.Properties.Title,
                                    Summary = c.Properties.Summary
                                }).ToList();

                categoryService.SetCategories(CategoriesResult);
            }
            
            Console.WriteLine();
            Console.WriteLine("***** ------ MindNose Iniciado ------ *****");
        }
    }
}