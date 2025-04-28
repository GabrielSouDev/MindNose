using RelationalGraph.Domain.Node;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace RelationalGraph.Application.Operations;
public class IAResponseFormat
{
    public static TermResult ResponseToObject(string response) //meta-llama/llama-3.3-70b-instruct:free
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
        var usageObj = JsonSerializer.Deserialize<Usage>(usage.GetRawText());

        if(termObj is null || usageObj is null)
            throw new ArgumentNullException(nameof(termObj), "Term object or Usage object is null.");

        termObj.Usage = new Usage()
        {
            prompt_tokens = usageObj.prompt_tokens,
            completion_tokens = usageObj.completion_tokens,
            total_tokens = usageObj.total_tokens
        };

        DEBUG(false, termObj);

        return termObj;
    }
    private static void DEBUG(bool debugMode, TermResult termObj)
    {
        if (debugMode)
        {
            Console.WriteLine($"Category: {termObj.Category}");
            Console.WriteLine($"Termo: {termObj.Term}");
            Console.WriteLine($"Resumo: {termObj.Summary}");
            Console.WriteLine($"Peso do termo na categoria: {termObj.WeigthCategoryToTerm}");
            Console.WriteLine("");

            foreach (var term in termObj.RelatedTerms)
            {
                Console.WriteLine($"Termo: {term.Term} ");
                Console.WriteLine($"Peso Term to Term: {term.WeigthTermToTerm}");
                Console.WriteLine("");
            }
            Console.WriteLine($"Prompt Token: {termObj.Usage.prompt_tokens}");
            Console.WriteLine($"Completion Token: {termObj.Usage.completion_tokens}");
            Console.WriteLine($"Total Token: {termObj.Usage.total_tokens}");
        }
    }
}