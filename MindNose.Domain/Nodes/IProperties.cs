using System.Text.Json.Serialization;

namespace MindNose.Domain.Nodes
{
    [JsonDerivedType(typeof(TermProperties), typeDiscriminator: "Term")]
    [JsonDerivedType(typeof(CategoryProperties), typeDiscriminator: "Category")]
    public interface IProperties
    {
        public string Title { get; set; }
        public string Summary { get; }
        public double[]? Embedding { get; set; }
        public int PromptTokens { get; set; }
        public int CompletionTokens { get; set; }
        public int TotalTokens { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}