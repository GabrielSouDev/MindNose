using System.Text.RegularExpressions;

namespace MindNose.Domain.Request;

public static class RequestFormat
{
    public static string NormalizeIdentifier(this string input) =>
        string.IsNullOrEmpty(input)
        ? string.Empty 
        : Regex.Replace(input.ToUpper(), @"[\s\-_]", "");
}
