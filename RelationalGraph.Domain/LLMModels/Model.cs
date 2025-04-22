public class Model
{
    public string Id { get; set; }
    public string Name { get; set; }
    public long Created { get; set; }
    public string Description { get; set; }
    public int Context_Length { get; set; }
    public Architecture Architecture { get; set; }
    public Pricing Pricing { get; set; }
    public TopProvider Top_Provider { get; set; }
    public object Per_Request_Limits { get; set; }
}
