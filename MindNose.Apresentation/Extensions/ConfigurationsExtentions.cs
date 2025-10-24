using MindNose.Domain.Configurations;


namespace MindNose.Apresentation.Extensions;

public static class ConfigurationsExtentions
{
    public static void AddConfigurations(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddEnvironmentVariables();

        builder.Services.Configure<OpenRouterSettings>(sp =>
        {
            sp.ProjectTitle = builder.Configuration["ProjectTitle"] ??
                              builder.Configuration["OpenRouterSettings:ProjectTitle"] ??
                              throw new Exception("Não foi possivel localizar Configurar o OpenRouterClient!");

            sp.ApiKey = builder.Configuration["ApiKey"] ??
                        builder.Configuration["OpenRouterSettings:ApiKey"] ??
                        throw new Exception("Não foi possivel localizar Configurar o OpenRouterClient!");

            sp.Url =  builder.Configuration["Url"] ??
                      builder.Configuration["OpenRouterSettings:Url"] ??
                      throw new Exception("Não foi possivel localizar Configurar o OpenRouterClient!");

            sp.site = builder.Configuration["Site"] ??
                      builder.Configuration["OpenRouterSettings:Site"] ??
                      throw new Exception("Não foi possivel localizar Configurar o OpenRouterClient!");
        });


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