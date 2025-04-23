namespace RelationalGraph.Domain.Node;

public class RelationshipProperties //IF Type = TERM, ESSA É A PROP USADA
{
    public string StartTerm { get; set; } = string.Empty;
    public string EndTerm { get; set; } = string.Empty;
    public double WeigthTermToTerm { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}