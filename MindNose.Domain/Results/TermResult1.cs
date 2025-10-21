using MindNose.Domain.Request;

namespace MindNose.Domain.Results;
public class TermResult : NodeResult
{
    private string _title = string.Empty;
    public override string Title
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
                _title = value.TermNormalize();
            }
        }
    }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}