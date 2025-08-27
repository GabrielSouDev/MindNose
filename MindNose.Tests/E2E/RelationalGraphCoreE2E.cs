using MindNose.Domain.Services;
using MindNose.Infrastructure.HttpClients;
using MindNose.Infrastructure.Persistence;
using MindNose.Tests.Integration;

namespace MindNose.Tests.E2E
{
    public class RelationalGraphCoreE2E
    {
        //private readonly RelationalGraphCoreController _relationalGraphCoreController;
        public RelationalGraphCoreE2E()
        {
            OpenRouterIntegration OpenRouterIntegration = new();
            Neo4jIntegration neo4jItntegration = new();

            var openRouterClient = new OpenRouterClient(OpenRouterIntegration.Options);
            var neo4jClient = new BoltNeo4jClient(neo4jItntegration.Options);

            var openRouterService = new OpenRouterService(openRouterClient);
            var neo4jService = new Neo4jService(neo4jClient);

            //_relationalGraphCoreController = new(openRouterService, neo4jService); 
        }
        [Fact]
        public void /*async Task*/ RelationalGraphCore() 
        {
            //refazer tudo para um httpclient da api
            //var response = await _relationalGraphCoreController.SearchAndCreateKnowledgeNode("Programação", "Javascript");

            //var resultOk = Assert.IsType<OkObjectResult>(response);
        }
    }
}
