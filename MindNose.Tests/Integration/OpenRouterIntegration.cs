using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Neo4j.Driver;
using MindNose.Domain.Operations;
using MindNose.Domain.Configurations;
using MindNose.Infrastructure.HttpClients;
using MindNose.Infrastructure.Persistence;
using MindNose.Domain.Request;

namespace MindNose.Tests.Integration;

[Collection("Sequential")]
public class OpenRouterIntegration
{
    public readonly IOptions<OpenRouterSettings> Options;
    public OpenRouterIntegration()
    {
        var apiProjectPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "..", "..", "..", "..",
            "RelationalGraph.API"
        );
        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        OpenRouterSettings openRouterSettings = new()
        {
            ProjectTitle = configuration["OpenRouterSettings:ProjectTitle"]!,
            ApiKey = configuration["OpenRouterSettings:ApiKey"]!,
            site = configuration["OpenRouterSettings:site"]!,
            Url = configuration["OpenRouterSettings:Url"]!
        };

        Options = new OptionsWrapper<OpenRouterSettings>(openRouterSettings);
    }
    [Fact]
    public async Task HttpClientOpenRouterConnection()
    {
        var httpClient = new OpenRouterClient(Options);

        var prompt = PromptFactory.NewKnowledgeNode(new LinksRequest() { Category = "Programação", Term = "Javascript"});
        var response = await httpClient.EnviarPrompt(prompt);

        Assert.NotNull(response);
    }
}
