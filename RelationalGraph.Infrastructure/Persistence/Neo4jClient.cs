using Microsoft.Extensions.Configuration;
using Neo4j.Driver;
using RelationalGraph.Application.Interfaces.Clients;
using Query = RelationalGraph.Application.Operations.Query;

namespace RelationalGraph.Infrastructure.Persistence;
public class Neo4jClient : INeo4jClient
{
    private readonly IDriver _driver;

    public Neo4jClient(IConfiguration config)
    {
        if (config == null)
            throw new ArgumentNullException(nameof(config));

        IConfigurationSection? openRouterConfig = config.GetSection("Neo4j");
        var uri = openRouterConfig["Url"] ?? 
            throw new ArgumentNullException(nameof(config));
        var user = openRouterConfig["Username"] ??
            throw new ArgumentNullException(nameof(config));
        var password = openRouterConfig["Password"] ??
            throw new ArgumentNullException(nameof(config));

        _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
    }

    public void Dispose()
    {
        _driver?.Dispose();
    }

    public async Task<List<string>> WriteToGraphAndReturnNode(Query query)
    {
        await using var session = _driver.AsyncSession();

        var result = await session.ExecuteWriteAsync(async tx =>
        {
            var cursor = await tx.RunAsync(query.CommandLine, query.Parameters);
            var records = await cursor.ToListAsync();

            //abstrair retorno para retornar json com id,label,propriedades[] e elementId.
            return records
                .Select(r => r["n"].As<INode>().Properties["name"].As<string>())
                .ToList();
        });

        return result;
    }
}