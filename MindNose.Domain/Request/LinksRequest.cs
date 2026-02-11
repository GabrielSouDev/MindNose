namespace MindNose.Domain.Request;

public class LinksRequest
{
    private string _category {get;set;} = string.Empty;
    public string CategoryId { get => _category.NormalizeIdentifier(); }
    public string Category
    {
        get => _category;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _category = string.Empty;
            }
            else
            {
                _category = value;
            }
        }
    }
    public string? CategorySummary { get; set; }
    public string TermId { get => _term.NormalizeIdentifier(); }
    private string _term { get; set; } = string.Empty;
    public string Term
    {
        get => _term;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _term = string.Empty;
            }
            else
            {
                _term = value;
            }
        }
    }
    public string? LLMModel { get; set; } = "qwen/qwen-turbo";
    public int? RelatedTermQuantity { get; set; } = 5;
    public int? LengthPath { get; set; } = 1;
    public int? Limit { get; set; } = 10;
    public int? Skip { get; set; } = 0;
}
