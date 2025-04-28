using RelationalGraph.Domain.Node;

namespace RelationalGraph.Application.Interfaces.Services
{
    public interface INeo4jService
    {
        Task<Link> CreateKnowledgeNode(string response);
        Task<Link> NodeIsExists(string category, string term);
    }
}