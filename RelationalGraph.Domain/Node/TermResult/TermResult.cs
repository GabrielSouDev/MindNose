namespace RelationalGraph.Domain.Node;
public class TermResult
{
    private string _category = string.Empty;
    private string _term = string.Empty;
    public string Category
    {
        get => _category;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _category = char.ToUpper(value[0]) + value.Substring(1).ToLower();
            }
            else
            {
                _category = string.Empty;
            }
        }
    }
    public string Term
    {
        get => _term;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _term = char.ToUpper(value[0]) + value.Substring(1).ToLower();
            }
            else
            {
                _term = string.Empty;
            }
        }
    }
    public string Summary { get; set; } = string.Empty;
    public double WeigthCategoryToTerm { get; set; }
    public List<RelatedTerm> RelatedTerms { get; set; } = new();
    public Usage Usage { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
