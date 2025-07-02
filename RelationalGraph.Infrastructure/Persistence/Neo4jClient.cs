using Neo4j.Driver;
using RelationalGraph.Domain.Node;
using RelationalGraph.Application.Interfaces.Clients;
using Query = RelationalGraph.Application.Operations.Query;
using RelationalGraph.Domain.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;


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

        var url = $"bolt://{_settings.Url}:{_settings.Port}";
        var token = AuthTokens.Basic(_settings.Username, _settings.Password);
        _driver = GraphDatabase.Driver(url, token);

        var connected = false;
        var retryLimit = 15;
        for (var i = 0; i < retryLimit; i++)
        {
            try
            {
                var connection = _driver.VerifyAuthenticationAsync(token).GetAwaiter().GetResult();

                Console.WriteLine("Conectado ao Banco de dados Neo4j!");
                connected = true;
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{i + 1} - Tentativa de conexão ao Neo4j falha!  Error: {ex.Message}");
                Thread.Sleep(5000);
            }
        }

        if(!connected) 
            throw new Exception("Não foi possivel conectar ao Banco de dados Neo4j.");
    }


    public void Dispose()
    {
        _driver?.Dispose();
    }
    public async Task<Link?> SearchInGraphAndReturnNode(Query query)
    {
        await using var session = _driver.AsyncSession();
        Link? result = await session.ExecuteWriteAsync(async tx => {

            var cursor = await tx.RunAsync(query.CommandLine, query.Parameters);
            var records = await cursor.ToListAsync();

            if (!records.Any())
                return null;

            Node category = GetCategory(records);
            Relationship crel = GetCategoryRelationship(records);
            Node startNode = GetNode(records);
            List<Node> relatedNodes = GetRelatedNodes(records);
            List<Relationship> rels = GetRelatedNodeRelationship(records);

            List<Node> nodes = new() { category, startNode };
            nodes.AddRange(relatedNodes);

            List<Relationship> relationships = new() { crel };
            relationships.AddRange(rels);

            return new Link()
            {
                Nodes = nodes,
                Relationships = relationships
            };
        });
        return result;
    }
    public async Task<Link> WriteInGraphAndReturnNode(Query query)
    {
        await using var session = _driver.AsyncSession();

        Link result = await session.ExecuteWriteAsync(async tx =>
        {
            var cursor = await tx.RunAsync(query.CommandLine, query.Parameters);
            var records = await cursor.ToListAsync();

            if (!records.Any())
                throw new Exception("Nenhum resultado foi retornado pela consulta.");

            Node category = GetCategory(records);
            Relationship crel = GetCategoryRelationship(records);
            Node startNode = GetNode(records);
            List<Node> relatedNodes = GetRelatedNodes(records);
            List<Relationship> rels = GetRelatedNodeRelationship(records);

            List<Node> nodes = new() { category, startNode };
            nodes.AddRange(relatedNodes);

            List<Relationship> relationships = new() { crel };
            relationships.AddRange(rels);

            return new Link
            {
                Nodes = nodes,
                Relationships = relationships
            };
        });
        return result;
    }

    private List<Node> GetRelatedNodes(List<IRecord> records)
    {
        return records.Select(r =>
        {
            var rNode = r["r"].As<INode>();
            var props = rNode.Properties.ToDictionary(p => p.Key, p => p.Value);


            return new Node
            {
                Id = rNode.Id,
                Label = rNode.Labels.FirstOrDefault() ?? "Title",
                Properties = new TermProperties
                {
                    Title = props.TryGetValue("Title", out var TitleVal) ? TitleVal.ToString()! : "",
                    Summary = props.TryGetValue("Summary", out var summaryVal) ? summaryVal.ToString()! : "Ainda não explorado!",
                    Usage = new Usage
                    {
                        prompt_tokens = props.TryGetValue("prompt_tokens", out var promptTokens) ? Convert.ToInt32(promptTokens) : 0,
                        completion_tokens = props.TryGetValue("completion_tokens", out var completionTokens) ? Convert.ToInt32(completionTokens) : 0,
                        total_tokens = props.TryGetValue("total_tokens", out var totalTokens) ? Convert.ToInt32(totalTokens) : 0
                    },
                    CreatedAt = props.TryGetValue("CreatedAt", out var createdAtVal) ? DateTime.Parse(createdAtVal.ToString()!) : DateTime.UtcNow
                },
                ElementId = rNode.ElementId
            };
        }).ToList();
    }

    private List<Relationship> GetRelatedNodeRelationship(List<IRecord> records)
    {
        return records.Select(r =>
        {
            var n = r["n"].As<INode>();
            var rel = r["rel"].As<IRelationship>();
            var rNode = r["r"].As<INode>();

            var relProps = rel.Properties.ToDictionary(p => p.Key, p => p.Value);
            var nProps = n.Properties.ToDictionary(p => p.Key, p => p.Value);
            var rProps = rNode.Properties.ToDictionary(p => p.Key, p => p.Value);

            return new Relationship
            {
                Id = rel.Id,
                ElementId = rel.ElementId,
                StartNodeId = n.Id,
                StartNodeElementId = n.ElementId,
                EndNodeId = rNode.Id,
                EndNodeElementId = rNode.ElementId,
                Type = rel.Type,
                Properties = new RelationshipProperties
                {
                    WeigthStartToEnd = relProps.TryGetValue("WeigthStartToEnd", out var weight) ? Convert.ToDouble(weight) : 0,
                    StartNode = nProps.TryGetValue("Title", out var TitleVal) ? TitleVal.ToString()! : "",
                    EndNode = rProps.TryGetValue("Title", out var rTitleVal) ? rTitleVal.ToString()! : "",
                    CreatedAt = relProps.TryGetValue("CreatedAt", out var relCreated) ? DateTime.Parse(relCreated.ToString()!) : DateTime.UtcNow
                }
            };
        }).ToList();
    }

    private Node GetNode(List<IRecord> records)
    {
        return records.Select(r =>
        {
            var n = r["n"].As<INode>();
            var props = n.Properties.ToDictionary(p => p.Key, p => p.Value);

            return new Node
            {
                Id = n.Id,
                Label = n.Labels.FirstOrDefault() ?? "Title",
                Properties = new TermProperties
                {
                    Title = props.TryGetValue("Title", out var TitleVal) ? TitleVal.ToString()! : "",
                    Summary = props.TryGetValue("Summary", out var summaryVal) ? summaryVal.ToString()! : "Sem resumo",
                    Usage = new Usage
                    {
                        prompt_tokens = props.TryGetValue("prompt_tokens", out var promptTokens) ? Convert.ToInt32(promptTokens) : 0,
                        completion_tokens = props.TryGetValue("completion_tokens", out var completionTokens) ? Convert.ToInt32(completionTokens) : 0,
                        total_tokens = props.TryGetValue("total_tokens", out var totalTokens) ? Convert.ToInt32(totalTokens) : 0
                    },
                    CreatedAt = props.TryGetValue("CreatedAt", out var createdAtVal) ? DateTime.Parse(createdAtVal.ToString()!) : DateTime.UtcNow
                },
                ElementId = n.ElementId
            };
        }).FirstOrDefault()!;
    }

    private Node GetCategory(List<IRecord> records)
    {
        return records.Select(r =>
        {
            var c = r["c"].As<INode>();
            var props = c.Properties.ToDictionary(p => p.Key, p => p.Value);

            return new Node
            {
                Id = c.Id,
                Label = c.Labels.FirstOrDefault()!,
                Properties = props.ContainsKey("CreatedAt")
                    ? new CategoryProperties {
                        Title = props.TryGetValue("Title", out var title) ? title.ToString()! : "",
                        CreatedAt = DateTime.Parse(props["CreatedAt"].ToString()!)
                    }
                    : new CategoryProperties(),
                ElementId = c.ElementId
            };
        }).FirstOrDefault()!;
    }

    private Relationship GetCategoryRelationship(List<IRecord> records)
    {
        return records.Select(r =>
        {
            var c = r["c"].As<INode>();
            var rel = r["crel"].As<IRelationship>();
            var n = r["n"].As<INode>();

            var relProps = rel.Properties.ToDictionary(p => p.Key, p => p.Value);
            var cProps = c.Properties.ToDictionary(p => p.Key, p => p.Value);
            var nProps = n.Properties.ToDictionary(p => p.Key, p => p.Value);

            return new Relationship
            {
                Id = rel.Id,
                ElementId = rel.ElementId,
                StartNodeId = c.Id,
                StartNodeElementId = c.ElementId,
                EndNodeId = n.Id,
                EndNodeElementId = n.ElementId,
                Type = rel.Type,
                Properties = new RelationshipProperties
                {
                    WeigthStartToEnd = relProps.TryGetValue("WeigthStartToEnd", out var weight) ? Convert.ToDouble(weight) : 0,
                    StartNode = cProps.TryGetValue("Title", out var catVal) ? catVal.ToString()! : "",
                    EndNode = nProps.TryGetValue("Title", out var TermVal) ? TermVal.ToString()! : "",
                    CreatedAt = relProps.TryGetValue("CreatedAt", out var relCreated) ? DateTime.Parse(relCreated.ToString()!) : DateTime.UtcNow
                }
            };
        }).FirstOrDefault()!;
    }
}