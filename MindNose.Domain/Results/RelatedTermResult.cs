using MindNose.Domain.Request;

namespace MindNose.Domain.Results;
public class RelatedTermResult : NodeResult
{
    public string? CanonicalDefinition { get; set; }
    public string? MainFunction { get; set; }
    public string? ConceptualCategory { get; set; }
    public double CategoryToRelatedTermWeigth { get; set; }
    public double InitialTermToRelatedTermWeigth { get; set; }

    public override string GetSummary()
    {
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(CanonicalDefinition)) parts.Add($"Definição Canônica: {CanonicalDefinition}");
        if (!string.IsNullOrWhiteSpace(MainFunction)) parts.Add($"Função Principal: {MainFunction}");
        if (!string.IsNullOrWhiteSpace(ConceptualCategory)) parts.Add($"Categoria Conceitual: {ConceptualCategory}");
        return string.Join("\n", parts);
    }
}
