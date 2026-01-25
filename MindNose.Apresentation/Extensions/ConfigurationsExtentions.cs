using MindNose.Domain.Configurations;
using System.Xml.Linq;


namespace MindNose.Apresentation.Extensions;

public static class ConfigurationsExtentions
{
    public static void AddConfigurations(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddEnvironmentVariables();

        builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWT") ?? throw new Exception("Não foi possivel Configurar o JWT."));

        builder.Services.Configure<Neo4jSettings>(builder.Configuration.GetSection("Neo4j") ?? throw new Exception("Não foi possivel Configurar o Neo4j."));

        builder.Services.Configure<OpenRouterSettings>(builder.Configuration.GetSection("OpenRouter") ??
                                                    throw new Exception("Não foi possivel Configurar o OpenRouter."));
    }
}
