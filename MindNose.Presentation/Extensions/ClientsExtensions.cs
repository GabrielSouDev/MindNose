using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MindNose.Domain.Configurations;
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

        builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var postgresSettings = builder.Configuration.GetSection("Postgres").Get<PostgresSettings>() ?? 
                                        throw new Exception("Não foi possivel configurar o Postgres.");

            options.UseNpgsql(postgresSettings.ConnectionString, b => b.MigrationsAssembly("MindNose.Infrastructure"));
        });
    }
}