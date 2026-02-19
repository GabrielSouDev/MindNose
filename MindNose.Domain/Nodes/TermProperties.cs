namespace MindNose.Domain.Nodes;

public class TermProperties : IProperties
{
    public string Title { get; set; } = string.Empty;
    public string Summary { get => GetSummary(); }
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int TotalTokens { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CanonicalDefinition { get; set; }
    public string? MainFunction { get; set; }
    public string? ConceptualCategory { get; set; }
    public double[]? Embedding { get; set; }

    private string GetSummary()
    {
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(CanonicalDefinition)) parts.Add($"Definição Canônica: {CanonicalDefinition}");
        if (!string.IsNullOrWhiteSpace(MainFunction)) parts.Add($"Função Principal: {MainFunction}");
        if (!string.IsNullOrWhiteSpace(ConceptualCategory)) parts.Add($"Categoria Conceitual: {ConceptualCategory}");
        return string.Join("\n", parts);
    }
}
