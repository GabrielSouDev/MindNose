using RelationalGraph.Domain.Node;
using RelationalGraph.Application.Interfaces;
using RelationalGraph.Application.Interfaces.Clients;
using RelationalGraph.Application.Interfaces.Services;
using RelationalGraph.Application.Operations;
using Microsoft.VisualBasic;
using System.Data;

namespace RelationalGraph.Application.Services
{
    public class Neo4jService : INeo4jService
    {
        private readonly INeo4jClient _neo4jClient;

        public Neo4jService(INeo4jClient neo4jClient)
        {
            _neo4jClient = neo4jClient;
        }

        public async Task<Link> CreateKnowledgeNode(string response)
        {
            var TermObject = IAResponseFormat.ResponseToObject(response);

            Query query = QueryFactory.CreateKnowledgeNode(TermObject);

            var result = await _neo4jClient.WriteInGraphAndReturnNode(query);
            return result;
        }

        public async Task<Link> NodeIsExists(string category, string term)
        {
            Query query = QueryFactory.SearchKnowledgeNode(category, term);

            var result = await _neo4jClient.SearchInGraphAndReturnNode(query);

            return result;
        }
    }
}
