using RelationalGraph.Application.Interfaces;
using RelationalGraph.Application.Interfaces.Clients;
using RelationalGraph.Application.Interfaces.Services;

namespace RelationalGraph.Application.Services
{
    public class Neo4jService : INeo4jService
    {
        private readonly INeo4jClient _neo4jClient;

        public Neo4jService(INeo4jClient neo4jClient)
        {
            _neo4jClient = neo4jClient;
        }
    }
}
