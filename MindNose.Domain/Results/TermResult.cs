using MindNose.Domain.Request;

namespace MindNose.Domain.Results;
public class TermResult : NodeResult
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}