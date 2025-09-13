using MindNose.Domain.Nodes;
using MindNose.Domain.Request;
using MindNose.Domain.Results;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MindNose.Domain.Operations;
public static class IAResponseFormat
{
    public static TermResult TermResultDeserializer(this string response) //meta-llama/llama-3.3-70b-instruct:free
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

        var parcialTermResult = JsonSerializer.Deserialize<ParcialTermResult>(innerJson!);
        var usage = JsonSerializer.Deserialize<Usage>(usageJson.GetRawText());

        if(parcialTermResult is null || usage is null)
            throw new ArgumentNullException(nameof(TermResult), "TermResult object is null in Deserializer!");

        TermResult termResult = new()
        {
            Category = parcialTermResult.Category,
            Term = parcialTermResult.Term,
            Summary = parcialTermResult.Summary,
            CreatedAt = parcialTermResult.CreatedAt,
            Usage = usage
        };

        foreach (var relatedTerms in parcialTermResult.RelatedTerms)
        {
            RelatedTerm rTerm = new()
            {
                Term = relatedTerms,
                CreatedAt = parcialTermResult.CreatedAt
            };
            termResult.RelatedTerms.Add(rTerm);
        }

        return termResult;
    }

    private class ParcialTermResult
    {

        public string Category { get; set; } = string.Empty;
        public string Term { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string[] RelatedTerms { get; set; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    private static void DEBUG(this TermResult termObj)
    {
        Console.WriteLine($"Category: {termObj.Category}");
        Console.WriteLine($"Termo: {termObj.Term}");
        Console.WriteLine($"Resumo: {termObj.Summary}");
        Console.WriteLine("");
    
        foreach (var term in termObj.RelatedTerms)
        {
            Console.WriteLine($"Termo: {term.Term} ");
        }
        Console.WriteLine("");
        Console.WriteLine($"Prompt Token: {termObj.Usage.prompt_tokens}");
        Console.WriteLine($"Completion Token: {termObj.Usage.completion_tokens}");
        Console.WriteLine($"Total Token: {termObj.Usage.total_tokens}");
    }
}