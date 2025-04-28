using System.Text.Json.Serialization;

namespace RelationalGraph.Domain.Node;
public class Node
{
    public long Id { get; set; }
    public string Label { get; set; } = string.Empty;
    [JsonConverter(typeof(PropertiesConverter))]
    public IProperties Properties { get; set; } = default!;
    public string ElementId { get; set; } = string.Empty;
}