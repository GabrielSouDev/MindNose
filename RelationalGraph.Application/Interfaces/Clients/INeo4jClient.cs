using RelationalGraph.Domain.Nodes;
using RelationalGraph.Domain.CMDs;

namespace RelationalGraph.Application.Interfaces.Clients;
public interface INeo4jClient : IDisposable
{
    Task<Links> WriteInGraphAndReturnLink(Query query);
    Task<Links?> SearchAndReturnLink(Query query);
}