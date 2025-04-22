using RelationalGraph.Domain.Node;
using RelationalGraph.Application.Operations;

namespace RelationalGraph.Application.Interfaces.Clients;
public interface INeo4jClient : IDisposable
{
    Task<List<Node>> WriteToGraphAndReturnNode(Query query);
}