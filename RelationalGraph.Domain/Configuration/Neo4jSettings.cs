namespace RelationalGraph.Domain.Configuration;

public class Neo4jSettings
{
    public string Url { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
}