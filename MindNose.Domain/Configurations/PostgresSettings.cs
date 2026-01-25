namespace MindNose.Domain.Configurations;
public class PostgresSettings
{
    public string Name { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public string Port { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConnectionString => $"Host={this.Host};Port={this.Port};Database={this.Name};Username={this.User};Password={this.Password}";
}