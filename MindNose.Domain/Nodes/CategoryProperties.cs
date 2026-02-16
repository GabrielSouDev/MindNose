namespace MindNose.Domain.Nodes
{
    public class CategoryProperties : IProperties
    {
        public string Title { get; set; } = string.Empty;
        public string Summary { get => GetSummary(); }
        public int PromptTokens { get; set; }
        public int CompletionTokens { get; set; }
        public int TotalTokens { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Function { get; set; }
        public string? Definition { get; set; }
        public double[]? Embedding { get; set; }

        private string GetSummary()
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(Definition)) parts.Add($"Definitionn: {Definition}");
            if (!string.IsNullOrWhiteSpace(Function)) parts.Add($"Function: {Function}");
            return string.Join("\n", parts);
        }
    }
}