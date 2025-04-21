using Microsoft.Extensions.Configuration;
using Neo4j.Driver;
using RelationalGraph.Application.DTO;
using RelationalGraph.Application.Interfaces.Clients;
using System.Diagnostics.CodeAnalysis;
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

    public async Task<List<Node>> WriteToGraphAndReturnNode(Query query)
    {
        await using var session = _driver.AsyncSession();

        var result = await session.ExecuteWriteAsync(async tx =>
        {
            var cursor = await tx.RunAsync(query.CommandLine, query.Parameters);
            var records = await cursor.ToListAsync();

            //abstrair retorno para retornar json com id,label,propriedades[] e elementId.
            return records.Select(r =>
            {
                var node = r["n"].As<INode>(); // Pega o nó
                var properties = node.Properties.ToDictionary(k => k.Key, v => v.Value); // Extrai as propriedades
                var elementId = node.Id; // ID do elemento (Node ID)
                var label = node.Labels.FirstOrDefault(); // Pega o primeiro label, se existir (caso haja múltiplos, você pode adaptar conforme necessário)
                return new Node()
                {
                    Id = elementId,
                    Label = label,
                    Properties = properties,
                    ElementId = elementId // ou você pode usar `node.Id`, se necessário
                };
            }).ToList();
        });

        return result;
    }
}