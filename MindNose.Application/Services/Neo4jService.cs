using MindNose.Domain.Nodes;
using MindNose.Domain.Interfaces.Clients;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.CMDs;
using MindNose.Domain.Results;
using MindNose.Domain.Request;
using MindNose.Domain.Exceptions;

namespace MindNose.Domain.Services
{
    public class Neo4jService : INeo4jService
    {
        private readonly INeo4jClient _neo4jClient;

        public Neo4jService(INeo4jClient neo4jClient)
        {
            _neo4jClient = neo4jClient;
        }

        public async Task<Links?> SaveTermResultAndReturnLinksAsync(TermResult TermObject)
        {
            var links = await _neo4jClient.CreateAndReturnLinksAsync(TermObject);

            return links;
        }

        public async Task<Links?> IfExistsReturnLinksAsync(LinksRequest request)
        {
            var result = await _neo4jClient.GetLinksAsync(request);

            return result;
        }
    }
}