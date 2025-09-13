using MindNose.Domain.Request;
using System.Text.RegularExpressions;
using static System.Formats.Asn1.AsnWriter;

namespace MindNose.Domain.Results;
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
    public double CategoryToRelatedTermWeigth { get; set; }
    public double InitialTermToRelatedTermWeigth { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
