using MindNose.Domain.Request;
using System.Text.RegularExpressions;

namespace MindNose.Domain.Results;
public class TermResult
{
    private string _category = string.Empty;
    private string _term = string.Empty;
    public string Category
    {
        get => _category;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _category = string.Empty;
            }
            else
            {
                _category = value.CategoryNormalize();
            }
        }
    }
    public string Term
    {
        get => _term;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _term = string.Empty;
            }
            else
            {
                _term = value.TermNormalize();
            }
        }
    }
    public string Summary { get; set; } = string.Empty;
    public double CategoryToTermWeigth { get; set; }
    public List<RelatedTerm> RelatedTerms { get; set; } = new();
    public Usage Usage { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
