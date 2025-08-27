using MindNose.Domain.Nodes;
using MindNose.Domain.Request;
using MindNose.Domain.TermResults;

namespace MindNose.Domain.Interfaces.Services
{
    public interface INeo4jService
    {
        Task<Links?> IfNodeExistsReturnLinks(LinksRequest request);
        Task<Links> SaveTermResultAndReturnIntoLinks(TermResult response);
    }
}