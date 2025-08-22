using MindNose.Domain.Nodes;
using MindNose.Domain.Interfaces.Clients;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Operations;
using MindNose.Domain.CMDs;
using MindNose.Domain.TermResults;

namespace MindNose.Domain.Services
{
    public class Neo4jService : INeo4jService
    {
        private readonly INeo4jClient _neo4jClient;

        public Neo4jService(INeo4jClient neo4jClient)
        {
            _neo4jClient = neo4jClient;
        }

        public async Task<Links> SaveTermResultAndReturnIntoLinks(TermResult TermObject)
        {

            Query query = QueryFactory.CreateKnowledgeNode(TermObject);

            var result = await _neo4jClient.WriteInGraphAndReturnLink(query);

            return result;
        }

        public async Task<Links?> IfNodeExistsReturnLinks(string category, string term)
        {
            Query query = QueryFactory.SearchKnowledgeNode(category, term);

            var result = await _neo4jClient.SearchAndReturnLink(query);

            return result;
        }
    }
}
