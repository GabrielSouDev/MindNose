namespace RelationalGraph.Application.DTO;
public class TermResult
{
    public string Term { get; set; }
    public string Summary { get; set; }
    public double WeigthCategoryToTerm { get; set; }
    public List<RelatedTerm> RelatedTerms { get; set; }
    public Usage Usage { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
