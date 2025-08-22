using MindNose.Domain.Nodes;
using MindNose.Domain.TermResults;

namespace MindNose.Domain.Interfaces.Services
{
    public interface INeo4jService
    {
        Task<Links> SaveTermResultAndReturnIntoLinks(TermResult response);
        Task<Links?> IfNodeExistsReturnLinks(string category, string term);
    }
}