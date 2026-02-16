namespace MindNose.Domain.Results;
public class LinksResult
{
    public CategoryResult Category { get; set; } = new();
    public TermResult Term { get; set; } = new();
    public List<RelatedTermResult> RelatedTerms { get; set; } = new();
    public Usage Usage { get; set; } = new();
    public bool WasCreated { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
