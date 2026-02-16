using MindNose.Domain.Request;
using System.Diagnostics;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MindNose.Domain.Results;
public abstract class NodeResult
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
    public IEnumerable<double>? Embedding { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual string GetSummary()
    {
        return string.Empty;
    }
}