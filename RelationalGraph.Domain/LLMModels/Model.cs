public class Model
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public long Created { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Context_Length { get; set; }
    public Architecture Architecture { get; set; } = new();
    public Pricing Pricing { get; set; } = new();
    public TopProvider Top_Provider { get; set; } = new();
    public object Per_Request_Limits { get; set; } = new();
}
