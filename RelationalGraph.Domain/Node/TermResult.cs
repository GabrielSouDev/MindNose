namespace RelationalGraph.Domain.Node;
public class TermResult
{
    public string Term { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public double WeigthCategoryToTerm { get; set; }
    public List<RelatedTerm> RelatedTerms { get; set; } = new();
    public Usage Usage { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
