using System.Text.Json;
using System.Text.RegularExpressions;

namespace RelationalGraph.Application.Operations;
public class IAResponseFormat
{
    public static TermResult ResponseToObject(string response)
    {
        var responseJson = JsonDocument.Parse(response);
        var content = responseJson
            .RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        var usage = responseJson
            .RootElement
            .GetProperty("usage");

        var match = Regex.Match(content, "```(json)?\\s*(.*?)\\s*```", RegexOptions.Singleline);
        var innerJson = match.Success ? match.Groups[2].Value : content;

        var termObj = JsonSerializer.Deserialize<TermResult>(innerJson);
        var usageObj = JsonSerializer.Deserialize<Usage>(usage);
        if (termObj is not null && usageObj is not null)
            termObj.Usage = usageObj;

        Console.WriteLine($"Termo: {termObj.Term}");
        Console.WriteLine($"Resumo: {termObj.Summary}");
        Console.WriteLine($"Peso do termo na categoria: {termObj.WeigthTermCategory}");
        Console.WriteLine("");

        foreach (var term in termObj.RelatedTerms)
        {
            Console.WriteLine($"Termo: {term.Term} ");
            Console.WriteLine($"Peso Category to Term: {term.WeigthCategoryToTerm}");
            Console.WriteLine($"Peso Term to Term: {term.WeigthTermToTerm}");
            Console.WriteLine("");
        }
        Console.WriteLine($"Prompt Token: {termObj.Usage.prompt_tokens}");
        Console.WriteLine($"Completion Token: {termObj.Usage.completion_tokens}");
        Console.WriteLine($"Total Token: {termObj.Usage.total_tokens}");
    }
}