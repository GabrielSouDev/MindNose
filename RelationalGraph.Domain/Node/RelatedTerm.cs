namespace RelationalGraph.Domain.Node;
public class RelatedTerm
{
    public string Term { get; set; } = string.Empty;
    public double WeigthCategoryToTerm { get; set; }
    public double WeigthTermToTerm { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
