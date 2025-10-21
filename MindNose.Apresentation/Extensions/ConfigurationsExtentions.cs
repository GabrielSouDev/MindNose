using MindNose.Domain.Configurations;


namespace MindNose.Apresentation.Extensions;

public static class ConfigurationsExtentions
{
    public static void AddConfigurations(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddEnvironmentVariables();

        builder.Services.Configure<OpenRouterSettings>(builder.Configuration.GetSection("OpenRouterSettings"));
        builder.Services.Configure<Neo4jSettings>(neo4j =>
        {
            neo4j.Host = builder.Configuration["DBHOST"] ??
                builder.Configuration["Neo4jSettings:Host"] ??
                throw new Exception("Não foi possivel localizar o host do Neo4j.");

            neo4j.Username = builder.Configuration["DBUSER"] ??
                builder.Configuration["Neo4jSettings:Username"] ??
                throw new Exception("Não foi possivel localizar o username do Neo4j.");

            neo4j.Password = builder.Configuration["DBPASSWORD"] ??
                builder.Configuration["Neo4jSettings:Password"] ??
                throw new Exception("Não foi possivel localizar o password do Neo4j.");
        });
    }
}