using MindNose.Domain.Request;
using System.Diagnostics;
using System.Globalization;

namespace MindNose.Domain.Results;
public class NodeResult
{
    public string TitleId { get => _title.NormalizeIdentifier(); }
    private string _title = string.Empty;
    public string Title
    {
        get => _title;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _title = string.Empty;
            }
            else
            {
                _title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
            }
        }
    }
    public string Summary { get; set; } = string.Empty;
}