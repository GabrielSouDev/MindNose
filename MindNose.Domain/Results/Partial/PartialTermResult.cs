namespace MindNose.Domain.Results.Partial;

public class PartialTermResult
{

    public string Category { get; set; } = string.Empty;
    public string CategorySummary { get; set; } = string.Empty;
    public string Term { get; set; } = string.Empty;
    public string TermSummary { get; set; } = string.Empty;
    public string[] RelatedTerms { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}