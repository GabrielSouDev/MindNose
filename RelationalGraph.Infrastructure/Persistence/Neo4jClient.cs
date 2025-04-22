using Microsoft.Extensions.Configuration;
using Neo4j.Driver;
using RelationalGraph.Domain.Node;
using RelationalGraph.Application.Interfaces.Clients;
using System.Diagnostics.CodeAnalysis;
using Query = RelationalGraph.Application.Operations.Query;
using RelationalGraph.Domain.Configuration;
using Microsoft.Extensions.Options;

namespace RelationalGraph.Infrastructure.Persistence;
public class Neo4jClient : INeo4jClient
{
    private readonly IDriver _driver;
    public readonly Neo4jSettings _settings;

    public Neo4jClient(IOptions<Neo4jSettings> settings)
    {
        _settings = settings.Value;

        if (_settings == null)
            throw new ArgumentNullException(nameof(_settings));

        _driver = GraphDatabase.Driver(_settings.Url, AuthTokens.Basic(_settings.Username, _settings.Password));
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