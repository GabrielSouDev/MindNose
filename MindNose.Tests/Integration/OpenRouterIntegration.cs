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
    private readonly PromptFactory _promptFactory;
    public OpenRouterIntegration(PromptFactory promptFactory)
    {
        _promptFactory = promptFactory;
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
    public async Task OpenRouterConnectionAsync()
    {
        var httpClient = new OpenRouterClient(Options);

        var request = new LinksRequest()
        {
            Category = "Programação",
            Term = "Javascript"
        };

        var prompt = _promptFactory.Node.NewTermResult(request);
        var response = await httpClient.EnviarPromptAsync(prompt, "mistralai/mistral-small-3.1-24b-instruct");

        Assert.NotNull(response);
    }
}
