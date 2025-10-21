using MindNose.Domain.Interfaces.Clients;
using MindNose.Infrastructure.HttpClients;
using MindNose.Infrastructure.Persistence;

namespace MindNose.Apresentation.Extensions;

public static class ClientsExtensions
{
    public static void AddClients(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<INeo4jClient, Neo4jClient>();
        builder.Services.AddScoped<IOpenRouterClient, OpenRouterClient>();
    }
}