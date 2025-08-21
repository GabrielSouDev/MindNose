using RelationalGraph.Domain.Nodes;
using RelationalGraph.Domain.TermResult;

namespace RelationalGraph.Application.Interfaces.Services
{
    public interface INeo4jService
    {
        Task<Links> SaveTermResultAndReturnIntoLinks(TermResult response);
        Task<Links?> IfNodeExistsReturnLinks(string category, string term);
    }
}