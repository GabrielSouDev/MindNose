using MindNose.Domain.Nodes;
using MindNose.Domain.CMDs;

namespace MindNose.Domain.Interfaces.Clients;
public interface INeo4jClient : IDisposable
{
    Task<Links> WriteInGraphAndReturnLink(Query query);
    Task<Links?> SearchAndReturnLink(Query query);
}