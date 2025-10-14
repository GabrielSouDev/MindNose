using MindNose.Domain.Nodes;
using MindNose.Domain.Request;
using MindNose.Domain.Results;

namespace MindNose.Domain.Interfaces.Clients;
public interface INeo4jClient : IDisposable
{
    Task<Links?> CreateAndReturnLinksAsync(TermResult termResult);
    Task<Links> GetLinksAsync(LinksRequest request);
}