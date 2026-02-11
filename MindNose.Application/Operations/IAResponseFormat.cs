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
    public static LinksResult TermResultDeserializer(this string response)
    {
        var (contentJson, usage) = response.ExtractContentAndUsage();

        var partialTermResult = JsonSerializer.Deserialize<PartialTermResult>(contentJson!);

        if(partialTermResult is null || usage is null)
            throw new ArgumentNullException(nameof(PartialTermResult), "TermResult object is null in Deserializer!");

        LinksResult termResult = new()
        {
            Category = new CategoryResult() { Title = partialTermResult.Category, Summary = partialTermResult.CategorySummary },
            Term = new TermResult() { Title = partialTermResult.Term, Summary = partialTermResult.TermSummary },
            CreatedAt = partialTermResult.CreatedAt,
            Usage = usage
        };

        foreach (var relatedTerms in partialTermResult.RelatedTerms)
        {
            RelatedTermResult rTerm = new()
            {
                Title = relatedTerms,
                CreatedAt = partialTermResult.CreatedAt
            };
            termResult.RelatedTerms.Add(rTerm);
        }

        return termResult;
    }

    public static LinksResult CategoryResultDeserializer(this string response)
    {
        var (contentJson, usage) = response.ExtractContentAndUsage();
        var partialCategoryResult = JsonSerializer.Deserialize<PartialCategoryResult>(contentJson!);

        if (partialCategoryResult is null || usage is null)
            throw new ArgumentNullException(nameof(PartialCategoryResult), "CategoryResult object is null in Deserializer!");

        LinksResult linkResult = new()
        {
            Category = new CategoryResult() 
            { 
                Title = partialCategoryResult.Category, 
                Summary = partialCategoryResult.Summary 
            },
            CreatedAt = partialCategoryResult.CreatedAt,
            Usage = usage
        };

        return linkResult;
    }

    public static LinksResult RelatedTermResultDeserializer(this string response, LinksResult linksResult)
    {
        var (contentJson, usage) = response.ExtractContentAndUsage();
        var partialTermResult = JsonSerializer.Deserialize<List<PartialRelatedTermResult>>(contentJson!);

        if (partialTermResult is null || usage is null)
            throw new ArgumentNullException(nameof(List<PartialRelatedTermResult>), "RelatedTermResult object is null in Deserializer!");

        for (int i = 0; i < linksResult.RelatedTerms.Count; i++)
        {
            var selectedTerm = partialTermResult.Where(ptr => ptr.RelatedTerm == linksResult.RelatedTerms[i].TitleId).First();
            linksResult.RelatedTerms[i].Summary = selectedTerm.RelatedTermSummary;
        }

        linksResult.Usage.TotalTokens += usage.TotalTokens;
        linksResult.Usage.PromptTokens += usage.PromptTokens;
        linksResult.Usage.CompletionTokens += usage.CompletionTokens;

        return linksResult;
    }

    private static void DEBUG(this LinksResult termObj)
    {
        Console.WriteLine($"Category: {termObj.Category.TitleId}");
        Console.WriteLine($"Resumo: {termObj.Category.Summary}");
        Console.WriteLine($"Termo: {termObj.Term.TitleId}");
        Console.WriteLine($"Resumo: {termObj.Term.Summary}");
        Console.WriteLine("");
    
        foreach (var term in termObj.RelatedTerms)
        {
            Console.WriteLine($"Termo: {term.TitleId} ");
            Console.WriteLine($"Termo: {term.Summary} ");
            Console.WriteLine("");
        }
        Console.WriteLine("");
        Console.WriteLine($"Prompt Token: {termObj.Usage.PromptTokens}");
        Console.WriteLine($"Completion Token: {termObj.Usage.CompletionTokens}");
        Console.WriteLine($"Total Token: {termObj.Usage.TotalTokens}");
    }
}