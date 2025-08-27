using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Neo4j.Driver;
using MindNose.Domain.CMDs;
using MindNose.Domain.Configurations;
using MindNose.Infrastructure.HttpClients;
using MindNose.Infrastructure.Persistence;

namespace MindNose.Tests.Integration
{
    public class Neo4jIntegration
    {

        public readonly IOptions<Neo4jSettings> Options;
        public Neo4jIntegration()
        {
            var apiProjectPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "..", "..", "..", "..",
                "RelationalGraph.API"
            );
            var configuration = new ConfigurationBuilder()
                .SetBasePath(apiProjectPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var neo4jSettings = new Neo4jSettings()
            {
                Host = configuration["DBHOST"] ?? configuration["Neo4jSettings:Host"]!,
                Username = configuration["DBUSER"] ?? configuration["Neo4jSettings:Username"]!,
                Password = configuration["DBPASSWORD"] ?? configuration["Neo4jSettings:Password"]!
            };

            Options = new OptionsWrapper<Neo4jSettings>(neo4jSettings);
        }
        [Fact]
        public void Neo4JConnection()
        {
            var client = new BoltNeo4jClient(Options);
            Assert.NotNull(client);
        }
    }
}
