using MindNose.Domain.Nodes;
using MindNose.Domain.Interfaces.Clients;
using Microsoft.Extensions.Options;
using MindNose.Domain.Configurations;
using Query = MindNose.Domain.CMDs.Query;
using Neo4jClient;
using MindNose.Domain.TermResults;
using Relationship = MindNose.Domain.Nodes.Relationship;
using Neo4jClient.Cypher;
using MindNose.Domain.Request;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection.Emit;
using System.Reflection;
using Neo4j.Driver;
using MindNose.Domain.Exceptions;
using System.Xml.Linq;
using INode = MindNose.Domain.Nodes.INode;
using System.Linq;
using IRelationship = MindNose.Domain.Nodes.IRelationship;

namespace MindNose.Infrastructure.Persistence;
public class BoltNeo4jClient : IBoltNeo4jClient
{
    public readonly IBoltGraphClient _client;
    public readonly Neo4jSettings _settings;

    public BoltNeo4jClient(IOptions<Neo4jSettings> settings)
    {
        _settings = settings.Value;

        if (_settings == null)
            throw new ArgumentNullException(nameof(_settings));

        _client = new BoltGraphClient(_settings.Host, _settings.Username, _settings.Password);
    }

    public async Task InitializeAsync()
    {
        await TryConnectAsync();
        await WarmUp();
    }
    private async Task TryConnectAsync()
    {
        var retryLimit = 15;
        var retrySleepTime = 5000;

        for (var i = 0; i < retryLimit; i++)
        {
            try
            {
                await _client.ConnectAsync();

                Console.WriteLine("Conectado ao Banco de dados Neo4j!");
                Console.WriteLine();
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{i + 1} - Tentativa de conexão ao Neo4j falha!  Error: {ex.Message}");
                Console.WriteLine();

                await Task.Delay(retrySleepTime);
            }
        }

        throw new Exception("Não foi possivel conectar ao Banco de dados Neo4j.");
    }

    private async Task WarmUp()
    {
        await _client.Cypher
            .Match("(n)")
            .Return(n => n.Count())
            .Limit(1)
            .ResultsAsync;

        Console.WriteLine("Warm-Up Completo!");
        Console.WriteLine();
    }

    public async Task<Links> CreateAndReturnLinksAsync(TermResult termResult)
    {
        var queryResultEnumerable = await _client.Cypher
            .WithParam("initialTerm", termResult)

            // Merge da categoria
            .Merge("(category:Category { Title: $initialTerm.Category })")
            .OnCreate().Set("category.CreatedAt = $initialTerm.CreatedAt")
            .OnMatch().Set("category.CreatedAt = coalesce(category.CreatedAt, $initialTerm.CreatedAt)")

            // Merge do termo principal
            .Merge("(term:Term { Title: $initialTerm.Term })")
            .OnCreate().Set(@"
                    term.Summary = $initialTerm.Summary,
                    term.CreatedAt = $initialTerm.CreatedAt,
                    term.PromptTokens = $initialTerm.Usage.prompt_tokens,
                    term.CompletionTokens = $initialTerm.Usage.completion_tokens,
                    term.TotalTokens = $initialTerm.Usage.total_tokens")
            .OnMatch().Set(@"
                    term.Summary = coalesce(term.Summary, $initialTerm.Summary),
                    term.CreatedAt = coalesce(term.CreatedAt, $initialTerm.CreatedAt),
                    term.PromptTokens = coalesce(term.PromptTokens, 0) + $initialTerm.Usage.prompt_tokens,
                    term.CompletionTokens = coalesce(term.CompletionTokens, 0) + $initialTerm.Usage.completion_tokens,
                    term.TotalTokens = coalesce(term.TotalTokens, 0) + $initialTerm.Usage.total_tokens")

            // Relacionamento categoria -> termo principal
            .Merge("(category)-[relationshipContains:CONTAINS]->(term)")
            .OnCreate().Set(@"
                    relationshipContains.WeigthStartToEnd = $initialTerm.WeigthCategoryToInitialTerm,
                    relationshipContains.CreatedAt = $initialTerm.CreatedAt")
            .OnMatch().Set(@"
                    relationshipContains.WeigthStartToEnd = coalesce(relationshipContains.WeigthStartToEnd, $initialTerm.WeigthCategoryToInitialTerm),
                    relationshipContains.CreatedAt = coalesce(relationshipContains.CreatedAt, $initialTerm.CreatedAt)")

            // Preparando termos relacionados
            .With("term, category, relationshipContains")
            .Unwind("$initialTerm.RelatedTerms", "relatedTermParam")

            // Merge dos termos relacionados
            .Merge("(relatedTerm:Term { Title: relatedTermParam.Term })")
            .OnCreate().Set("relatedTerm.CreatedAt = relatedTermParam.CreatedAt")
            .OnMatch().Set("relatedTerm.CreatedAt = coalesce(relatedTerm.CreatedAt, relatedTermParam.CreatedAt)")

            // Relacionamento termo principal -> termos relacionados
            .Merge("(term)-[relationshipRelated:RELATED_TO]->(relatedTerm)")
            .OnCreate().Set(@"
                    relationshipRelated.WeigthStartToEnd = relatedTermParam.WeigthInitialTermToRelatedTerm,
                    relationshipRelated.CreatedAt = relatedTermParam.CreatedAt")
            .OnMatch().Set(@"
                    relationshipRelated.WeigthStartToEnd = coalesce(relationshipRelated.WeigthStartToEnd, relatedTermParam.WeigthInitialTermToRelatedTerm),
                    relationshipRelated.CreatedAt = coalesce(relationshipRelated.CreatedAt, relatedTermParam.CreatedAt)")

            // Relacionamento categoria -> termos relacionados
            .Merge("(category)-[relationshipContainsRelated:CONTAINS]->(relatedTerm)")
            .OnCreate().Set(@"
                    relationshipContainsRelated.WeigthStartToEnd = relatedTermParam.WeigthCategoryToRelatedTerm,
                    relationshipContainsRelated.CreatedAt = relatedTermParam.CreatedAt")
            .OnMatch().Set(@"
                    relationshipContainsRelated.WeigthStartToEnd = coalesce(relationshipContainsRelated.WeigthStartToEnd, relatedTermParam.WeigthCategoryToRelatedTerm),
                    relationshipContainsRelated.CreatedAt = coalesce(relationshipContainsRelated.CreatedAt, relatedTermParam.CreatedAt)")

            // Retorno
            .Return((term, category, relationshipContains, relationshipRelated, relatedTerm, relationshipContainsRelated) => new 
            {
                CategoryId = Return.As<long>("id(category)"),
                CategoryElementId = Return.As<string>("elementId(category)"),
                CategoryLabels = Return.As<List<string>>("labels(category)"),
                CategoryProperties = category.As<CategoryProperties>(),

                TermId = Return.As<long>("id(term)"),
                TermElementId = Return.As<string>("elementId(term)"),
                TermLabels = Return.As<List<string>>("labels(term)"),
                TermProperties = term.As<TermProperties>(),

                RelatedTermId = Return.As<long>("id(relatedTerm)"),
                RelatedTermElementId = Return.As<string>("elementId(relatedTerm)"),
                RelatedTermLabels = Return.As<List<string>>("labels(relatedTerm)"),
                RelatedTermProperties = relatedTerm.As<TermProperties>(),

                RelationshipContainsId = Return.As<List<long>>("id(relationshipContains)"),
                RelationshipContainsElementId = Return.As<List<string>>("elementId(relationshipContains)"),
                RelationshipContainsStartNodeId = Return.As<long>("startNodeId(relationshipContains)"),
                RelationshipContainsStartNodeElementId = Return.As<string>("startNodeElementId(relationshipContains)"),
                RelationshipContainsEndNodeId = Return.As<long>("endNodeId(relationshipContains)"),
                RelationshipContainsEndNodeElementId = Return.As<string>("endNodeElementId(relationshipContains)"),
                RelationshipContainsType = Return.As<List<string>>("type(relationshipContains)"),
                RelationshipContainsProperties = Return.As<List<RelationshipProperties>>("[r IN relationshipContains | r]"),

                RelationshipRelatedId = Return.As<List<long>>("id(relationshipRelated)"),
                RelationshipRelatedElementId = Return.As<List<string>>("elementId(relationshipRelated)"),
                RelationshipRelatedStartNodeId = Return.As<long>("startNodeId(relationshipRelated)"),
                RelationshipRelatedStartNodeElementId = Return.As<string>("startNodeElementId(relationshipRelated)"),
                RelationshipRelatedEndNodeId = Return.As<long>("endNodeId(relationshipRelated)"),
                RelationshipRelatedEndNodeElementId = Return.As<string>("endNodeElementId(relationshipRelated)"),
                RelationshipRelatedType = Return.As<List<string>>("type(relationshipRelated)"),
                RelationshipRelatedProperties = Return.As<List<RelationshipProperties>>("[r IN relationshipRelated | r]"),

                RelationshipContainsRelatedId = Return.As<List<long>>("id(relationshipContainsRelated)"),
                RelationshipContainsRelatedElementId = Return.As<List<string>>("elementId(relationshipContainsRelated)"),
                RelationshipContainsRelatedStartNodeId = Return.As<long>("startNodeId(relationshipContainsRelated)"),
                RelationshipContainsRelatedStartNodeElementId = Return.As<string>("startNodeElementId(relationshipContainsRelated)"),
                RelationshipContainsRelatedEndNodeId = Return.As<long>("endNodeId(relationshipContainsRelated)"),
                RelationshipContainsRelatedEndNodeElementId = Return.As<string>("endNodeElementId(relationshipContainsRelated)"),
                RelationshipContainsRelatedType = Return.As<List<string>>("type(relationshipContainsRelated)"),
                RelationshipContainsRelatedProperties = Return.As<List<RelationshipProperties>>("[r IN relationshipContainsRelated | r]")
            }).ResultsAsync;

        var links = MapToLinks(queryResultEnumerable, termResult.Usage);
        return links;
    }

    public async Task<Links> GetLinks(LinksRequest request)
    {
        var queryResultEnumerable = await _client.Cypher
            .WithParam("request", request)

            .Match($"(category:Category {{ Title: $request.Category }})-[relationshipContains:CONTAINS*1..{request.LengthPath}]->(term:Term {{ Title: $request.Term }})").

            .OptionalMatch($"(term)-[relationshipRelated:RELATED_TO*1..{request.LengthPath}]->(relatedTerm)")
            .OptionalMatch($"(category)-[relationshipContainsRelated:CONTAINS*1..{request.LengthPath}]->(relatedTerm)")

            .Limit(request.Limit)
            .Skip(request.Skip)
            .Return((term, category, relationshipContains, relationshipRelated, relationshipContainsRelated, relatedTerm) => new
            {
                CategoryId = Return.As<long>("id(category)"),
                CategoryElementId = Return.As<string>("elementId(category)"),
                CategoryLabels = Return.As<List<string>>("labels(category)"),
                CategoryProperties = category.As<CategoryProperties>(),
                TermId = Return.As<long>("id(term)"),
                TermElementId = Return.As<string>("elementId(term)"),
                TermLabels = Return.As<List<string>>("labels(term)"),
                TermProperties = term.As<TermProperties>(),
                RelatedTermId = Return.As<long>("id(relatedTerm)"),
                RelatedTermElementId = Return.As<string>("elementId(relatedTerm)"),
                RelatedTermLabels = Return.As<List<string>>("labels(relatedTerm)"),
                RelatedTermProperties = relatedTerm.As<TermProperties>(),
                RelationshipContainsId = Return.As<IEnumerable<long>>("[r IN relationshipContains | id(r)]"),
                RelationshipContainsElementId = Return.As<IEnumerable<string>>("[r IN relationshipContains | elementId(r)]"),
                RelationshipContainsStartNodeId = Return.As<IEnumerable<long>>("[r IN relationshipContains | id(startNode(r))]"),
                RelationshipContainsStartNodeElementId = Return.As<IEnumerable<string>>("[r IN relationshipContains | elementId(startNode(r))]"),
                RelationshipContainsEndNodeId = Return.As<IEnumerable<long>>("[r IN relationshipContains | id(endNode(r))]"),
                RelationshipContainsEndNodeElementId = Return.As<IEnumerable<string>>("[r IN relationshipContains | elementId(endNode(r))]"),
                RelationshipContainsType = Return.As<IEnumerable<string>>("[r IN relationshipContains | type(r)]"),
                RelationshipContainsProperties = Return.As<IEnumerable<RelationshipProperties>>("[r IN relationshipContains | r]"),
                RelationshipRelatedId = Return.As<IEnumerable<long>>("[r IN relationshipRelated | id(r)]"),
                RelationshipRelatedElementId = Return.As<IEnumerable<string>>("[r IN relationshipRelated | elementId(r)]"),
                RelationshipRelatedStartNodeId = Return.As<IEnumerable<long>>("[r IN relationshipRelated | id(startNode(r))]"),
                RelationshipRelatedStartNodeElementId = Return.As<IEnumerable<string>>("[r IN relationshipRelated | elementId(startNode(r))]"),
                RelationshipRelatedEndNodeId = Return.As<IEnumerable<long>>("[r IN relationshipRelated | id(endNode(r))]"),
                RelationshipRelatedEndNodeElementId = Return.As<IEnumerable<string>>("[r IN relationshipRelated | elementId(endNode(r))]"),
                RelationshipRelatedType = Return.As<IEnumerable<string>>("[r IN relationshipRelated | type(r)]"),
                RelationshipRelatedProperties = Return.As<IEnumerable<RelationshipProperties>>("[r IN relationshipRelated | r]"),
                RelationshipContainsRelatedId = Return.As<IEnumerable<long>>("[r IN relationshipContainsRelated | id(r)]"),
                RelationshipContainsRelatedElementId = Return.As<IEnumerable<string>>("[r IN relationshipContainsRelated | elementId(r)]"),
                RelationshipContainsRelatedStartNodeId = Return.As<IEnumerable<long>>("[r IN relationshipContainsRelated | id(startNode(r))]"),
                RelationshipContainsRelatedStartNodeElementId = Return.As<IEnumerable<string>>("[r IN relationshipContainsRelated | elementId(startNode(r))]"),
                RelationshipContainsRelatedEndNodeId = Return.As<IEnumerable<long>>("[r IN relationshipContainsRelated | id(endNode(r))]"),
                RelationshipContainsRelatedEndNodeElementId = Return.As<IEnumerable<string>>("[r IN relationshipContainsRelated | elementId(endNode(r))]"),
                RelationshipContainsRelatedType = Return.As<IEnumerable<string>>("[r IN relationshipContainsRelated | type(r)]"),
                RelationshipContainsRelatedProperties = Return.As<IEnumerable<RelationshipProperties>>("[r IN relationshipContainsRelated | r]"),
            }).ResultsAsync;

        var links = MapToLinks(queryResultEnumerable);
        return links;
    }

    private Links MapToLinks(IEnumerable<dynamic> queryResultEnumerable, Usage? usage = null)
    {
        if (!queryResultEnumerable.Any())
            throw new LinkNotFoundException();

        var nodes = new List<INode>();
        var relationships = new List<IRelationship>();

        foreach (dynamic queryResult in queryResultEnumerable)
        {
            var queryProperties = queryResult.GetType().GetProperties();

            foreach (var property in queryProperties)
            {
                if (!IsIEnumerable(property))
                {
                    if (property.Name.Contains("Category"))
                        nodes.Add(new CategoryNode
                        {
                            Id = queryResult.CategoryId,
                            ElementId = queryResult.CategoryElementId,
                            Label = queryResult.CategoryLabels,
                            Properties = queryResult.CategoryProperties
                        });

                    if (property.Name.Contains("Term"))
                        nodes.Add(new TermNode
                        {
                            Id = queryResult.TermId,
                            ElementId = queryResult.TermElementId,
                            Label = queryResult.TermLabels,
                            Properties = queryResult.TermProperties
                        });
                    if (property.Name.Contains("RelatedTerm"))
                        nodes.Add(new TermNode
                        {
                            Id = queryResult.RelatedTermId,
                            ElementId = queryResult.RelatedTermElementId,
                            Label = queryResult.RelatedTermLabels,
                            Properties = queryResult.RelatedTermProperties
                        });
                }
                else
                {
                    var relationshipsDynamicValue = property.GetValue(queryResult);

                    if (relationshipsDynamicValue is not null && relationshipsDynamicValue is IEnumerable<dynamic> enumerable && relationshipsDynamicValue is not string)
                    {
                        foreach (var relationship in enumerable)
                        {
                            if (relationship is RelationshipProperties rel)
                            {
                                if (property.Name == "RelationshipContainsProperties")
                                {
                                    var ids = ((IEnumerable<long>)queryResult.RelationshipContainsId).ToArray();
                                    var elementIds = ((IEnumerable<string>)queryResult.RelationshipContainsElementId).ToArray();
                                    var types = ((IEnumerable<string>)queryResult.RelationshipContainsType).ToArray();
                                    var startNodeIds = ((IEnumerable<long>)queryResult.RelationshipContainsStartNodeId).ToArray();
                                    var startNodeElementIds = ((IEnumerable<string>)queryResult.RelationshipContainsStartNodeElementId).ToArray();
                                    var endNodeIds = ((IEnumerable<long>)queryResult.RelationshipContainsEndNodeId).ToArray();
                                    var endNodeElementIds = ((IEnumerable<string>)queryResult.RelationshipContainsEndNodeElementId).ToArray();

                                    int i = 0;
                                    foreach (var rela in enumerable)
                                    {
                                        relationships.Add(new Relationship()
                                        {
                                            Id = ids[i],
                                            ElementId = elementIds[i],
                                            Type = types[i],
                                            StartNodeId = startNodeIds[i],
                                            StartNodeElementId = startNodeElementIds[i],
                                            EndNodeId = endNodeIds[i],
                                            EndNodeElementId = endNodeElementIds[i],
                                            Properties = rela
                                        });
                                        i++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
        //List<Node> nodes = new List<Node>();
        //List<Relationship> relationships = new List<Relationship>();

        //foreach (var row in queryResultEnumerable)
        //{
        //    var termNode = new Node();
        //    var categoryNode = new Node();
        //    var relatedTermNode = new Node();

        //    var relationshipContains = new Relationship();
        //    var relationshipRelated = new Relationship();
        //    var relationshipContainsRelated = new Relationship();

        //    if (PropertyExistsInDynamicAnonymousObj("CategoryElementId", row))
        //    {
        //        categoryNode = new Node
        //        {
        //            Id = row.CategoryId,
        //            ElementId = row.CategoryElementId,
        //            Label = row.CategoryLabels,
        //            Properties = row.CategoryProperties
        //        };
        //    }

        //    if (PropertyExistsInDynamicAnonymousObj("TermElementId", row))
        //    {
        //        termNode = new Node
        //        {
        //            Id = row.TermId,
        //            ElementId = row.TermElementId,
        //            Label = row.TermLabels,
        //            Properties = row.TermProperties
        //        };
        //    }

        //    if (PropertyExistsInDynamicAnonymousObj("RelatedTermElementId", row))
        //    {
        //        relatedTermNode = new Node
        //        {
        //            Id = row.RelatedTermId,
        //            ElementId = row.RelatedTermElementId,
        //            Label = row.RelatedTermLabels,
        //            Properties = row.RelatedTermProperties
        //        };
        //    }

        //    if (relatedTermNode.Properties is TermProperties termProperties)
        //        if(string.IsNullOrEmpty(termProperties.Summary))
        //            termProperties.Summary = "Ainda não explorado!";

        //    nodes.AddRange([termNode, categoryNode, relatedTermNode]);

        //    if (PropertyExistsInDynamicAnonymousObj("RelationshipContainsElementId", row))
        //    {
        //        foreach (var itemRow in row.RelationshipContainsElementId)
        //        {
        //            relationshipContains = new Relationship
        //            {
        //                Id = itemRow.RelationshipContainsId,
        //                ElementId = itemRow.RelationshipContainsElementId,
        //                Type = itemRow.RelationshipContainsType,
        //                StartNodeId = itemRow.CategoryId,
        //                StartNodeElementId = itemRow.CategoryElementId,
        //                EndNodeId = itemRow.TermId,
        //                EndNodeElementId = itemRow.TermElementId,
        //                Properties = itemRow.RelationshipContainsProperties
        //            };
        //        }
        //    }

        //    if (PropertyExistsInDynamicAnonymousObj("RelationshipRelatedElementId", row))
        //    {
        //        foreach (var itemRow in row.RelationshipContainsElementId)
        //        {
        //            relationshipRelated = new Relationship
        //            {
        //                Id = itemRow.RelationshipRelatedId,
        //                ElementId = itemRow.RelationshipRelatedElementId,
        //                Type = itemRow.RelationshipRelatedType,
        //                StartNodeId = itemRow.TermId,
        //                StartNodeElementId = itemRow.TermElementId,
        //                EndNodeId = itemRow.RelatedTermId,
        //                EndNodeElementId = itemRow.RelatedTermElementId,
        //                Properties = itemRow.RelationshipRelatedProperties
        //            };
        //        }
        //    }

        //    if (PropertyExistsInDynamicAnonymousObj("RelationshipContainsRelatedElementId", row))
        //    {
        //        foreach (var itemRow in row.RelationshipContainsElementId)
        //        {
        //            relationshipContainsRelated = new Relationship
        //            {
        //                Id = itemRow.RelationshipContainsRelatedId,
        //                ElementId = itemRow.RelationshipContainsRelatedElementId,
        //                Type = itemRow.RelationshipContainsRelatedType,
        //                StartNodeId = itemRow.CategoryId,
        //                StartNodeElementId = itemRow.CategoryElementId,
        //                EndNodeId = itemRow.RelatedTermId,
        //                EndNodeElementId = itemRow.RelatedTermElementId,
        //                Properties = itemRow.RelationshipContainsRelatedProperties
        //            };
        //        }
        //    }
        //    relationships.AddRange([relationshipContains, relationshipRelated, relationshipContainsRelated]);
        //}
        //foreach(var node in nodes.Where(n => n is TermProperties))
        //{
        //    var summary = ((TermProperties)node.Properties).Summary;

        //    if(string.IsNullOrEmpty(summary))
        //        summary = "Ainda não explorado!";
        //}



        var links = new Links();

        links.Nodes.AddRange(
            nodes.DistinctBy(n => n.ElementId)
            .Where(n => !String.IsNullOrEmpty(n.ElementId)));

        links.Relationships.AddRange(
            relationships.DistinctBy(r => r.ElementId)
            .Where(r => !String.IsNullOrEmpty(r.ElementId)));

        if (usage is null)
            usage = new Usage();

        links.Usage = usage;

        return links;
    }

    private bool IsIEnumerable(PropertyInfo propertyInfo) =>
    propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>);

    private bool PropertyExistsInDynamicAnonymousObj(string propertyString, dynamic obj)
    {
        var property = obj.GetType().GetProperty(propertyString);
        return property != null;
    }
}