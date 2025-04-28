using RelationalGraph.Domain.Node;
using RelationalGraph.Application.Operations;

namespace RelationalGraph.Application.Interfaces.Clients;
public interface INeo4jClient : IDisposable
{
    Task<Link> WriteInGraphAndReturnNode(Query query);
    Task<Link?> SearchInGraphAndReturnNode(Query query);
}