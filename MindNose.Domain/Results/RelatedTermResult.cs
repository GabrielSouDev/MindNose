using MindNose.Domain.Request;

namespace MindNose.Domain.Results;
public class RelatedTermResult : NodeResult
{
    public double CategoryToRelatedTermWeigth { get; set; }
    public double InitialTermToRelatedTermWeigth { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
