using Microsoft.AspNetCore.Identity;
using MindNose.Application.Services;
using MindNose.Domain.Interfaces.Clients;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Nodes;
using MindNose.Domain.Request.User;
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
            var postgresService = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>(); 
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

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
                var CategoriesResult = categories.Nodes
                    .Select(c => c.Properties)
                    .OfType<CategoryProperties>()
                    .Select(props => new CategoryResult
                    {
                        Title = props.Title,
                        Definition = props.Definition,
                        Function = props.Function,
                        Embedding = props.Embedding,
                        CreatedAt = props.CreatedAt
                    })
                    .ToList();

                categoryService.SetCategories(CategoriesResult);
            }

            var adminRequest = configuration.GetSection("AdminUser").Get<UserRequest>();
            await postgresService.SeedDataAsync(userManager, roleManager, adminRequest!);

            Console.WriteLine();
            Console.WriteLine("***** ------ MindNose Iniciado ------ *****");
        }
    }
}