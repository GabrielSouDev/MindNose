namespace RelationalGraph.Domain.Node;

public class TermProperties //IF LABEL = TERM, ESSA É A PROP USADA
{
    public string Term { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public double WeigthCategoryToTerm { get; set; }
    public Usage Usage { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
