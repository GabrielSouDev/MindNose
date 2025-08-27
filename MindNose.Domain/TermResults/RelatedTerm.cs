using MindNose.Domain.Request;
using System.Text.RegularExpressions;

namespace MindNose.Domain.TermResults;
public class RelatedTerm
{
    private string _term = string.Empty;
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
    public double WeigthCategoryToRelatedTerm { get; set; }
    public double WeigthInitialTermToRelatedTerm { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
