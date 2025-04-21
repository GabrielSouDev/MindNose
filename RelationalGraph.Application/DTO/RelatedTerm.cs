namespace RelationalGraph.Application.DTO;
public class RelatedTerm
{
    public string Term { get; set; }
    public double WeigthCategoryToTerm { get; set; }
    public double WeigthTermToTerm { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
