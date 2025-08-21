using System.Text.Json.Serialization;

namespace RelationalGraph.Domain.Nodes;

public class Relationship
{
    public long Id { get; set; } // ID interno do relacionamento
    public string ElementId { get; set; } = string.Empty;   // ID estável do relacionamento
    public long StartNodeId { get; set; } // ID interno do nó de origem
    public string StartNodeElementId { get; set; } = string.Empty;  // ID estável do nó de origem
    public long EndNodeId { get; set; }   // ID interno do nó de destino
    public string EndNodeElementId { get; set; } = string.Empty;    // ID estável do nó de destino
    public string Type { get; set; } = string.Empty;    // Tipo do relacionamento
    public RelationshipProperties Properties { get; set; } = new();
}
