using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNose.Domain.Operations;

public static class RequestFormat
{
    public static string ToPascalCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(input.ToLower());
    }
}
