namespace RelationalGraph.Domain.Node;
public class RelatedTerm
{
    private string _term = string.Empty;
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
    public double WeigthTermToTerm { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
