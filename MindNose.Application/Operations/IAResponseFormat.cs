using MindNose.Domain.Results;
using MindNose.Domain.Results.Partial;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MindNose.Domain.Operations;
public static partial class IAResponseFormat
{
    public static LinksResult TermResultDeserializer(this string response)
    {
        var responseJson = JsonDocument.Parse(response);
        var content = responseJson
            .RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        var usageJson = responseJson
            .RootElement
            .GetProperty("usage");

        var match = Regex.Match(content!, "```(json)?\\s*(.*?)\\s*```", RegexOptions.Singleline);
        var innerJson = match.Success ? match.Groups[2].Value : content;

        var partialTermResult = JsonSerializer.Deserialize<PartialTermResult>(innerJson!);
        var usage = JsonSerializer.Deserialize<Usage>(usageJson.GetRawText());

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
        var responseJson = JsonDocument.Parse(response);
        var content = responseJson
            .RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        var usageJson = responseJson
            .RootElement
            .GetProperty("usage");

        var match = Regex.Match(content!, "```(json)?\\s*(.*?)\\s*```", RegexOptions.Singleline);
        var innerJson = match.Success ? match.Groups[2].Value : content;

        var partialCategoryResult = JsonSerializer.Deserialize<PartialCategoryResult>(innerJson!);
        var usage = JsonSerializer.Deserialize<Usage>(usageJson.GetRawText());

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
        var responseJson = JsonDocument.Parse(response);
        var content = responseJson
            .RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        var usageJson = responseJson
            .RootElement
            .GetProperty("usage");

        var match = Regex.Match(content!, "```(json)?\\s*(.*?)\\s*```", RegexOptions.Singleline);
        var innerJson = match.Success ? match.Groups[2].Value : content;

        var partialTermResult = JsonSerializer.Deserialize<List<PartialRelatedTermResult>>(innerJson!);
        var usage = JsonSerializer.Deserialize<Usage>(usageJson.GetRawText());

        if (partialTermResult is null || usage is null)
            throw new ArgumentNullException(nameof(List<PartialRelatedTermResult>), "RelatedTermResult object is null in Deserializer!");

        for (int i = 0; i < linksResult.RelatedTerms.Count; i++)
        {
            var selectedTerm = partialTermResult.Where(ptr => ptr.RelatedTerm == linksResult.RelatedTerms[i].Title).First();
            linksResult.RelatedTerms[i].Summary = selectedTerm.RelatedTermSummary;
        }

        linksResult.Usage.total_tokens += usage.total_tokens;
        linksResult.Usage.prompt_tokens += usage.prompt_tokens;
        linksResult.Usage.completion_tokens += usage.completion_tokens;

        return linksResult;
    }

    private static void DEBUG(this LinksResult termObj)
    {
        Console.WriteLine($"Category: {termObj.Category.Title}");
        Console.WriteLine($"Resumo: {termObj.Category.Summary}");
        Console.WriteLine($"Termo: {termObj.Term.Title}");
        Console.WriteLine($"Resumo: {termObj.Term.Summary}");
        Console.WriteLine("");
    
        foreach (var term in termObj.RelatedTerms)
        {
            Console.WriteLine($"Termo: {term.Title} ");
            Console.WriteLine($"Termo: {term.Summary} ");
            Console.WriteLine("");
        }
        Console.WriteLine("");
        Console.WriteLine($"Prompt Token: {termObj.Usage.prompt_tokens}");
        Console.WriteLine($"Completion Token: {termObj.Usage.completion_tokens}");
        Console.WriteLine($"Total Token: {termObj.Usage.total_tokens}");
    }
}