using MindNose.Domain.Nodes;
using MindNose.Domain.CMDs;
using MindNose.Domain.TermResults;
using MindNose.Domain.Request;

namespace MindNose.Domain.Interfaces.Clients;
public interface IBoltNeo4jClient
{
    Task InitializeAsync();
    Task<Links> CreateAndReturnLinksAsync(TermResult termResult);
    Task<Links> GetLinks(LinksRequest request);
}