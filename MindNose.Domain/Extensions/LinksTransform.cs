using MindNose.Domain.DTO.Cytoscape;
using MindNose.Domain.Nodes;

namespace MindNose.Domain.Extensions;

public static class LinksTransform
{
    public static CytoscapeDTO LinksToDTO(this Links links)
    {
        CytoscapeDTO linksDTO = new();

        linksDTO.Elements.Nodes = NodesToDTO(links.Nodes);
        linksDTO.Elements.Edges = RelationshipToDTO(links.Relationships);

        linksDTO.WasCreated = links.WasCreated;

        return linksDTO;
    }

    private static List<NodeDTO> NodesToDTO(this List<INode> nodes)
    {
        var nodesDTO = new List<NodeDTO>();

        nodes.ForEach(n =>
        {
             var node = new NodeDTO()
             {
                 Data = new NodeDataDTO
                 {
                     Id = n.ElementId,
                     Label = string.Join(", ", n.Label)
                 }
             };

             if (n is TermNode)
             {
                 var nodeProperties = (TermProperties)n.Properties;

                 node.Data.Extra = new Dictionary<string, object>()
                {
                    { "Title", nodeProperties.Title },
                    { "Summary", nodeProperties.Summary },
                    { "CreatedAt", nodeProperties.CreatedAt }
                };
             }
             else if (n is CategoryNode)
             {
                 var nodeProperties = (CategoryProperties)n.Properties;

                 node.Data.Extra = new Dictionary<string, object>()
                {
                    { "Title", nodeProperties.Title },
                    { "Summary", nodeProperties.Summary },
                    { "CreatedAt", nodeProperties.CreatedAt }
                };
             };

            nodesDTO.Add(node);
        });

        return nodesDTO;
    }

    private static List<EdgeDTO> RelationshipToDTO(this List<IRelationship> relationships)
    {
        relationships = relationships.OrderByDescending(e => ((RelationshipProperties)e.Properties).WeightStartToEnd).ToList();
        var relationshipsDTO = new List<EdgeDTO>();

        relationships.ForEach(r =>
         {
             var edgeProperties = (RelationshipProperties)r.Properties;

             var edge = new EdgeDTO()
             {
                 Data = new EdgeDataDTO
                 {
                     Id = r.ElementId,
                     Label = r.Type,
                     Source = r.StartNodeElementId,
                     Target = r.EndNodeElementId,
                     Extra = new Dictionary<string, object>()
                    {
                        {"WeightStartToEnd",edgeProperties.WeightStartToEnd},
                        {"CreatedAt",edgeProperties.CreatedAt}
                    }
                 }
             };

             relationshipsDTO.Add(edge);
         });

        return relationshipsDTO;
    }
}