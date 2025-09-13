using System.Text.Json.Serialization;

namespace MindNose.Domain.Nodes
{
    [JsonDerivedType(typeof(RelationshipProperties), typeDiscriminator: "Relationship")]
    public interface IRelationshipProperties
    {
        public DateTime CreatedAt { get; set; }
    }
}