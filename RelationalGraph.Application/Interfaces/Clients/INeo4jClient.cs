using RelationalGraph.Application.Operations;

namespace RelationalGraph.Application.Interfaces.Clients;
public interface INeo4jClient : IDisposable
{
    Task<List<string>> WriteToGraphAndReturnNode(Query query);
}