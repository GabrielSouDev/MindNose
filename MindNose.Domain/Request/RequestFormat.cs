using System.Text.RegularExpressions;

namespace MindNose.Domain.Request;

public static class RequestFormat
{
    public static string CategoryNormalize(this string input)
    {
        var words = input
            .Split(new[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(w => char.ToUpper(w[0]) + (w.Length > 1 ? w.Substring(1).ToLower() : ""));

        return string.Join(" ", words);
    }

    public static string TermNormalize(this string input) =>
        Regex.Replace(input.ToUpper(), @"[\s\-_]", "");
}
