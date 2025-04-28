namespace RelationalGraph.Domain.Configuration;

public class Neo4jSettings
{
    public string Url { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}