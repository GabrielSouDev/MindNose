using Neo4j.Driver;
using MindNose.Domain.Nodes;
using MindNose.Domain.Interfaces.Clients;
using Microsoft.Extensions.Options;
using MindNose.Domain.Configurations;
using MindNose.Domain.Results;
using MindNose.Domain.Request;
using Query = MindNose.Domain.CMDs.Query;
using MindNose.Domain.Exceptions;
using MindNose.Domain.Interfaces.Commons;

namespace MindNose.Infrastructure.Persistence;
public class Neo4jClient : INeo4jClient, IInitializable
{
    private readonly IDriver _client;
    private readonly IAuthToken _token;
    public readonly Neo4jSettings _settings;

    public Neo4jClient(IOptions<Neo4jSettings> settings)
    {
        _settings = settings.Value;

        if (_settings == null)
            throw new ArgumentNullException(nameof(_settings));

        _token = AuthTokens.Basic(_settings.Username, _settings.Password);
        _client = GraphDatabase.Driver(_settings.Host, _token);
    }

    public async Task InitializeAsync()
    {
        await TryConnectAsync();

        await SimpleWarmUpAsync();
    }

    public void Dispose()
    {
        _client?.Dispose();
    }

    private async Task TryConnectAsync()
    {
        var retryLimit = 15;
        var retrySleepTime = 5000;
        for (var i = 0; i < retryLimit; i++)
        {
            try
            {
                var connection = await _client.VerifyAuthenticationAsync(_token);

                Console.WriteLine("- Conectado ao Banco de dados Neo4j!");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"# {i + 1} - Tentativa de conexão ao Neo4j falha!  Error: {ex.Message}");

                await Task.Delay(retrySleepTime);
            }
        }
        throw new Exception("Não foi possivel conectar ao Banco de dados Neo4j.");
    }

    private async Task SimpleWarmUpAsync()
    {
        var query = CypherFactory.WarmUpWithOutResults();

        using var session = _client.AsyncSession();

        var cursor = await session.RunAsync(query.CommandLine);

        await Task.WhenAny(cursor.ConsumeAsync());
        Console.WriteLine("- Simple WarmUp Finalizado!");
    }

    private async Task<List<IRecord>> ExecuteAsync(Query query)
    {
        using var session = _client.AsyncSession();

        var cursor = await session.RunAsync(query.CommandLine, query.Parameters);

        return await cursor.ToListAsync();
    }
    public async Task<Links> GetLinksAsync(LinksRequest request)
    {
        var query = CypherFactory.GetLinks(request);

        var results = await ExecuteAsync(query);

        if (!results.Any())
            throw new LinksNotFoundException();

        var links = results.MapToLinks();

        return links;
    }
    public async Task<Links?> CreateAndReturnLinksAsync(LinksResult termResult)
    {
        var query = CypherFactory.CreateLinks(termResult);

        var results = await ExecuteAsync(query);

        if (!results.Any())
            return null;

        var links = results.MapToLinks();
        links.Usage = termResult.Usage;
        links.WasCreated = termResult.WasCreated;

        return links;
    }

    public async Task<Links?> GetCategories()
    {
        Query query = CypherFactory.GetCategories();

        var results = await ExecuteAsync(query);

        if (!results.Any())
            return null;

        var links = results.MapToLinks();

        return links;
    }

    public async Task<Links?> GetCategoryNodeAsync(string category)
    {
        Query query = CypherFactory.GetCategory(category);

        var results = await ExecuteAsync(query);

        if (!results.Any())
            return null;

        var links = results.MapToLinks();

        return links;
    }

    public async Task<Links?> CreateCategoryAndReturnLinks(LinksResult? categoryLinks)
    {
        var query = CypherFactory.CreateCategory(categoryLinks);

        var results = await ExecuteAsync(query);

        if (!results.Any())
            return null;

        var links = results.MapToLinks();

        return links;
    }
}