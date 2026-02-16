using MindNose.Domain.Request;

namespace MindNose.Domain.Results;
public class CategoryResult : NodeResult
{
    public string? Function {  get; set; }
    public string? Definition { get; set; }
    public override string GetSummary()
    {
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(Definition)) parts.Add($"Definição: {Definition}");
        if (!string.IsNullOrWhiteSpace(Function)) parts.Add($"Função: {Function}");
        return string.Join("\n", parts);
    }

}