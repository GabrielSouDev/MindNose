using MindNose.Domain.Request;

namespace MindNose.Domain.Results;
public class RelatedTermResult : NodeResult
{
    private string _Title = string.Empty;
    public override string Title
    {
        get => _Title;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _Title = string.Empty;
            }
            else
            {
                _Title = value.TermNormalize();
            }
        }
    }
    public double CategoryToRelatedTermWeigth { get; set; }
    public double InitialTermToRelatedTermWeigth { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
