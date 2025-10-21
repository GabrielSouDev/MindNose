using MindNose.Domain.Nodes;
using Neo4j.Driver;
using INodeDriver = Neo4j.Driver.INode;
using IRelationshipDriver = Neo4j.Driver.IRelationship;
using INode = MindNose.Domain.Nodes.INode;
using IRelationship = MindNose.Domain.Nodes.IRelationship;
using NodeProperty = MindNose.Domain.Consts.NodeProperty;
using RelationshipProperty = MindNose.Domain.Consts.RelationshipProperty;
using MindNose.Domain.Consts;

namespace MindNose.Infrastructure.Persistence;

public static class Neo4jExtensions
{
    public static Links MapToLinks(this List<IRecord> results)
    {
        var links = new Links();
        links.Nodes.AddRange(results.GetNodes(NodeType.Term));
        links.Nodes.AddRange(results.GetNodes(NodeType.Category));
        links.Nodes.AddRange(results.GetNodes(NodeType.RelatedTerm));

        links.Relationships.AddRange(
            results.GetRelationships(RelationshipType.RelationshipContains)
        );

        links.Relationships.AddRange(
            results.GetRelationships(RelationshipType.RelationshipRelated)
        );

        links.Relationships.AddRange(
            results.GetRelationships(RelationshipType.RelationshipContainsRelated)
        );

        return links.RemoveDuplicates();
    }

    private static Links RemoveDuplicates(this Links linksWithDuplicates)
    {
        var links = new Links();
        links.Nodes.AddRange(
            linksWithDuplicates.Nodes
                .Select(n => n)
                .DistinctBy(n => n.ElementId)
        );

        links.Relationships.AddRange(
            linksWithDuplicates.Relationships
                .Select(n => n)
                .DistinctBy(r => r.ElementId)
        );

        return links;
    }

    private static List<INode> GetNodes(this List<IRecord> recordList, string nodeType)
    {
        return recordList
            .Where(record => record.ContainsKey(nodeType))
            .Select(record =>
        {
            var nodeInfo = record[nodeType].As<INodeDriver>();
            var propertiesInfo = nodeInfo.Properties.ToDictionary(p => p.Key, p => p.Value);

            var properties = nodeInfo.GetNodeProperties(nodeType, propertiesInfo);

            return (INode)(nodeType == NodeType.Category
                ? new CategoryNode()
                {
                    ElementId = nodeInfo.ElementId,
                    Label = nodeInfo.Labels.ToList(),
                    Properties = (CategoryProperties)properties
                }
                : new TermNode()
                {
                    ElementId = nodeInfo.ElementId,
                    Label = nodeInfo.Labels.ToList(),
                    Properties = (TermProperties)properties
                }
            );
        }).ToList();
    }

    private static IProperties GetNodeProperties(this INodeDriver node, string nodeType, Dictionary<string, object> propertiesInfo)
    {
        return nodeType == NodeType.Category
            ? new CategoryProperties()
            {
                Title = propertiesInfo.TryGetValue(NodeProperty.Title, out var categoryTitle) ? categoryTitle.ToString()! : "Null",
                Summary = propertiesInfo.TryGetValue(NodeProperty.Summary, out var categorySummary) ? categorySummary.ToString()! : "Ainda Não Explorado!",
                PromptTokens = propertiesInfo.TryGetValue(NodeProperty.PromptTokens, out var categoryPromptTokens) ? Convert.ToInt32(categoryPromptTokens) : 0,
                CompletionTokens = propertiesInfo.TryGetValue(NodeProperty.CompletionTokens, out var categoryCompletionTokens) ? Convert.ToInt32(categoryCompletionTokens) : 0,
                TotalTokens = propertiesInfo.TryGetValue(NodeProperty.TotalTokens, out var categoryTotalTokens) ? Convert.ToInt32(categoryTotalTokens) : 0,
                CreatedAt = propertiesInfo.TryGetValue(NodeProperty.CreatedAt, out var categoryCreatedAt) ? DateTime.Parse(categoryCreatedAt.ToString()!) : DateTime.UtcNow
            }
            : new TermProperties()
            {
                Title = propertiesInfo.TryGetValue(NodeProperty.Title, out var termTitle) ? termTitle.ToString()! : "Null",
                Summary = propertiesInfo.TryGetValue(NodeProperty.Summary, out var termSummary) ? termSummary.ToString()! : "Ainda Não Explorado!",
                PromptTokens = propertiesInfo.TryGetValue(NodeProperty.PromptTokens, out var termPromptTokens) ? Convert.ToInt32(termPromptTokens) : 0,
                CompletionTokens = propertiesInfo.TryGetValue(NodeProperty.CompletionTokens, out var termCompletionTokens) ? Convert.ToInt32(termCompletionTokens) : 0,
                TotalTokens = propertiesInfo.TryGetValue(NodeProperty.TotalTokens, out var termTotalTokens) ? Convert.ToInt32(termTotalTokens) : 0,
                CreatedAt = propertiesInfo.TryGetValue(NodeProperty.CreatedAt, out var termCreatedAt) ? DateTime.Parse(termCreatedAt.ToString()!) : DateTime.UtcNow
            };
    }

    private static List<IRelationship> GetRelationships(this List<IRecord> recordList, string relationshipType)
    {
        var relationshipList = new List<IRelationship>();
        var pathId = 1;

        foreach (var record in recordList)
        {
            if (!record.ContainsKey(relationshipType))
                continue;

            var relationshipValue = record[relationshipType];

            if (relationshipValue is IPath path)
            {
                foreach (var relationship in path.Relationships)
                {
                    relationshipList.Add(RelationshipsMap(relationship));

                    relationshipList[pathId - 1].PathId = pathId;
                }
                pathId++;

            }
            else if(relationshipValue is IRelationshipDriver relationship)
            {
                var mapped = RelationshipsMap(relationship);
                mapped.PathId = pathId;
                relationshipList.Add(mapped);

                pathId++;
            }
        }
        return relationshipList;
    }

    private static IRelationship RelationshipsMap(IRelationshipDriver relationship)
    {
        var relationshipPropertiesInfo = relationship.Properties.ToDictionary(p => p.Key, p => p.Value);
        var relationshipProperties = relationshipPropertiesInfo.GetRelationshipProperties();

        return new Relationship
        {
            ElementId = relationship.ElementId,
            StartNodeElementId = relationship.StartNodeElementId,
            EndNodeElementId = relationship.EndNodeElementId,
            Type = relationship.Type,
            Properties = (RelationshipProperties)relationshipProperties
        };
    }

    private static IRelationshipProperties GetRelationshipProperties(this Dictionary<string, object> relationshipPropertiesInfo)
    {
        return new RelationshipProperties
        {
            WeightStartToEnd = relationshipPropertiesInfo.TryGetValue(RelationshipProperty.WeightStartToEnd, out var weight)
                                                                          ? Convert.ToDouble(weight)
                                                                          : 0,
            CreatedAt = relationshipPropertiesInfo.TryGetValue(RelationshipProperty.CreatedAt, out var relCreated)
                                                                    ? DateTime.Parse(relCreated.ToString()!)
                                                                    : DateTime.UtcNow
        };
    }
}