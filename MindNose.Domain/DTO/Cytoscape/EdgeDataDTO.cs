namespace MindNose.Domain.DTO.Cytoscape;
public class EdgeDataDTO
{
    public string Id { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Target { get; set; } = string.Empty;
    public string? Label { get; set; }
    public Dictionary<string, object>? Extra { get; set; } // Para metadados opcionais
}