using MindNose.Domain.Nodes;
using MindNose.Domain.Request;
using MindNose.Domain.Results;

namespace MindNose.Domain.Interfaces.Clients;
public interface INeo4jClient : IDisposable
{
    Task<Links?> CreateAndReturnLinksAsync(LinksResult termResult);
    Task<Links?> GetCategories();
    Task<Links> GetLinksAsync(LinksRequest request);
    Task<Links?> GetCategoryNodeAsync(string category);
    Task<Links?> CreateCategoryAndReturnLinks(LinksResult? categoryLinks);
}