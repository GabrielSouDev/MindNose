using MindNose.Domain.Nodes;
using MindNose.Domain.Interfaces.Clients;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.CMDs;
using MindNose.Domain.TermResults;
using MindNose.Domain.Request;
using MindNose.Domain.Exceptions;

namespace MindNose.Domain.Services
{
    public class Neo4jService : INeo4jService
    {
        private readonly IBoltNeo4jClient _neo4jClient;

        public Neo4jService(IBoltNeo4jClient neo4jClient)
        {
            _neo4jClient = neo4jClient;
        }

        public async Task<Links> SaveTermResultAndReturnIntoLinks(TermResult TermObject)
        {
            var links = await _neo4jClient.CreateAndReturnLinksAsync(TermObject);

            return links;
        }

        public async Task<Links?> IfNodeExistsReturnLinks(LinksRequest request)
        {
            var result = await _neo4jClient.GetLinks(request);

            //TermNode? initialTerm = result.Nodes?.Where(n => n is TermNode term && term.Properties.Title == request.Term).FirstOrDefault();

            //if (initialTerm is null)
            //    return null;
            
            return result;
        }
    }
}