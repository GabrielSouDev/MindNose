using System.Text.Json.Serialization;

namespace MindNose.Domain.Nodes
{
    [JsonDerivedType(typeof(TermProperties), typeDiscriminator: "Term")]
    [JsonDerivedType(typeof(CategoryProperties), typeDiscriminator: "Category")]
    public interface IProperties
    {
        public DateTime CreatedAt { get; set; }
    }
}