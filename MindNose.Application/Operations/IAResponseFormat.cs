using MindNose.Domain.Results;
using MindNose.Domain.Results.Partial;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MindNose.Domain.Operations;
public static partial class IAResponseFormat
{
    private static readonly Regex JsonBlockRegex =
        new(@"```(json)?\s*(.*?)\s*```", RegexOptions.Singleline | RegexOptions.Compiled);

    public static (string ContentJson, Usage Usage) ExtractContentAndUsage(this string response)
    {
        using var doc = JsonDocument.Parse(response);
        var root = doc.RootElement;

        var content = root
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? string.Empty;

        var usage = JsonSerializer.Deserialize<Usage>(
            root.GetProperty("usage").GetRawText()
        ) ?? throw new JsonException("Usage object is null.");

        var match = JsonBlockRegex.Match(content);
        var contentJson = match.Success ? match.Groups[2].Value : content;

        return (contentJson, usage);
    }

    public static (TermResultParcial, Usage) TermResultDeserializer(this string response)
    {
        var (contentJson, usage) = response.ExtractContentAndUsage();

        var partialTermResult = JsonSerializer.Deserialize<TermResultParcial>(contentJson!);

        if (partialTermResult is null || usage is null)
            throw new ArgumentNullException(nameof(PartialTermResult), "TermResult object is null in Deserializer!");

        return (partialTermResult, usage);
    }

    public static (CategoryResultParcial, Usage) CategoryResultDeserializer(this string response)
    {
        var (contentJson, usage) = response.ExtractContentAndUsage();
        var partialCategoryResult = JsonSerializer.Deserialize<CategoryResultParcial>(contentJson!);

        if (partialCategoryResult is null || usage is null)
            throw new ArgumentNullException(nameof(CategoryResultParcial), "CategoryResult object is null in Deserializer!");

        return (partialCategoryResult, usage);
    }

    public static (IEnumerable<RelatedTermResultParcial>, Usage) RelatedTermResultDeserializer(this string response, LinksResult linksResult)
    {
        var (contentJson, usage) = response.ExtractContentAndUsage();
        var partialTermResult = JsonSerializer.Deserialize<IEnumerable<RelatedTermResultParcial>>(contentJson!);

        if (partialTermResult is null || usage is null)
            throw new ArgumentNullException(nameof(IEnumerable<RelatedTermResultParcial>), "RelatedTermResult object is null in Deserializer!");

        return (partialTermResult, usage);
    }
}

public class TermResultParcial
{
    public string? CanonicalDefinition { get; set; }
    public string? MainFunction { get; set; }
    public string? ConceptualCategory { get; set; }
    public IEnumerable<string>? RelatedTerms { get; set; }
}

public class RelatedTermResultParcial
{
    public string Title { get; set; } = string.Empty;
    public string? CanonicalDefinition { get; set; }
    public string? MainFunction { get; set; }
    public string? ConceptualCategory { get; set; }
}

public class CategoryResultParcial
{
    public string? Definition { get; set; }
    public string? Function { get; set; }
}