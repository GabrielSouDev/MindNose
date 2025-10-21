namespace MindNose.Domain.Results.Partial;

public class PartialCategoryResult
{
    public string Category { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}