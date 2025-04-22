using RelationalGraph.Domain.Node;
using RelationalGraph.Application.Interfaces;
using RelationalGraph.Application.Interfaces.Clients;
using RelationalGraph.Application.Interfaces.Services;
using RelationalGraph.Application.Operations;

namespace RelationalGraph.Application.Services
{
    public class Neo4jService : INeo4jService
    {
        private readonly INeo4jClient _neo4jClient;

        public Neo4jService(INeo4jClient neo4jClient)
        {
            _neo4jClient = neo4jClient;
        }

        public async Task<List<Node>> CreateKnowledgeNode(string response)
        {
            var TermObject = IAResponseFormat.ResponseToObject(response);
            Query query = Query.CreateKnowledgeNode(TermObject);

            var result = await _neo4jClient.WriteToGraphAndReturnNode(query);
            return result;
        }
    }
}
