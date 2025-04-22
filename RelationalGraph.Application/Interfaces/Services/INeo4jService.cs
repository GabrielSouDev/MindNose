using RelationalGraph.Domain.Node;

namespace RelationalGraph.Application.Interfaces.Services
{
    public interface INeo4jService
    {
        Task<List<Node>> CreateKnowledgeNode(string response);
    }
}