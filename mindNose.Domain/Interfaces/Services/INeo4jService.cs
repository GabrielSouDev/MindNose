using MindNose.Domain.Nodes;
using MindNose.Domain.Request;
using MindNose.Domain.Results;

namespace MindNose.Domain.Interfaces.Services
{
    public interface INeo4jService
    {
        Task<Links?> IfExistsReturnLinksAsync(LinksRequest request);
        Task<Links?> SaveTermResultAndReturnLinksAsync(TermResult response);
    }
}